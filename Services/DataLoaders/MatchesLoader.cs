using System.Diagnostics.CodeAnalysis;
using Football.Data;
using Football.Entities;
using Football.Entities.Enums;
using Football.ExternalServices.Clients.Models;
using Football.ExternalServices.Interfaces;
using Football.Services.Odds.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Football.Services.DataLoaders;

public class MatchesLoader
{
    private readonly DatabaseContext _context;
    private readonly IFootballDataClient _footballDataClient;
    private readonly IOddsService _oddsService;
    private readonly ILogger<MatchesLoader> _logger;

    public MatchesLoader(
        DatabaseContext context,
        IFootballDataClient footballDataClient,
        IOddsService oddsService,
        ILogger<MatchesLoader> logger)
    {
        _context = context;
        _footballDataClient = footballDataClient;
        _oddsService = oddsService;
        _logger = logger;
    }

    // NOTE: Update scores for recently finished matches
    // POSSIBLE IMPROVEMENT:
    // - Load matches in batches using GetMatches method and using min and max dates from existing matches
    // - When loaded match is not found we can delete it from the database, but it's better to keep it for history
    public async Task LoadRecentAsync()
    {
        var matches = await _context.Matches
            .Where(x => x.Date <= DateTime.UtcNow && x.Status == MatchStatus.Scheduled)
            .ToArrayAsync();

        foreach (var match in matches)
        {
            MatchModel loadedMatch = null;
            try
            {
                loadedMatch = await _footballDataClient.GetMatch(match.ExternalId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load match {MatchId}", match.Id);
            }

            if (loadedMatch == null)
            {
                match.IsDeleted = true;
                continue;
            }

            match.Date = loadedMatch.UtcDate;
            match.Status = MatchStatus.Finished;
            match.Winner = loadedMatch.Score.Winner switch {
                "HOME_TEAM" => MatchWinner.HomeTeam,
                "AWAY_TEAM" => MatchWinner.AwayTeam,
                "DRAW" => MatchWinner.Draw,
                _ => null
            };
            match.FullTimeHomeScore = loadedMatch.Score.FullTime.Home;
            match.FullTimeAwayScore = loadedMatch.Score.FullTime.Away;
            match.HalfTimeHomeScore = loadedMatch.Score.HalfTime?.Home;
            match.HalfTimeAwayScore = loadedMatch.Score.HalfTime?.Away;
        }

        await _context.SaveChangesAsync();
    }

    // NOTE: Load upcoming matches since the last match in the database
    // POSSIBLE IMPROVEMENT:
    // - For performance reasons I decided to load only one week of upcoming matches
    public async Task LoadUpcomingAsync()
    {
        var matches = await _context.Matches.MaxAsync(x => (DateTime?)x.Date) ?? DateTime.UtcNow.AddDays(-30);
        var from = matches.Date;
        var to = from.AddDays(60);

        var leagues = await _context.Leagues
            .Select(x => new { x.Code, x.Id })
            .ToArrayAsync();

        foreach (var league in leagues)
        {
            IEnumerable<MatchModel> loadedMatches = [];
            try
            {
                loadedMatches = await _footballDataClient.GetMatches(league.Code, from, to);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load upcoming matches for league {LeagueCode}", league);
            }

            await ProcessLoadedMatches(league.Id, loadedMatches);
        }

        await _context.SaveChangesAsync();
    }

    // POSSIBLE IMPROVEMENT:
    // - We can use hashtable to store teams and avoid traversing the array for each team
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private async Task ProcessLoadedMatches(Guid leagueId, IEnumerable<MatchModel> loadedMatches)
    {
        var teamsFromMatches = loadedMatches
            .SelectMany(x => new[] {x.HomeTeam, x.AwayTeam})
            .Select(x => x.Id)
            .ToArray();

        var teams = await _context.Teams
            .Where(x => teamsFromMatches.Contains(x.ExternalId))
            .Select(x => new {x.Id, x.ExternalId})
            .ToArrayAsync();

        foreach (var loadedMatch in loadedMatches)
        {
            var homeTeamId = GetTeamId(loadedMatch.HomeTeam);
            var awayTeamId = GetTeamId(loadedMatch.AwayTeam);
            var ods = await _oddsService.GetMatchOdds();

            var match = new Match
            {
                Id = Guid.NewGuid(),
                ExternalId = loadedMatch.Id,
                Date = loadedMatch.UtcDate,
                Status = loadedMatch.Status switch {
                    "SCHEDULED" => MatchStatus.Scheduled,
                    "FINISHED" => MatchStatus.Finished,
                    _ => MatchStatus.Scheduled
                },
                HomeTeamId = awayTeamId,
                AwayTeamId = homeTeamId,
                WinOdds = ods.Win,
                DrawOdds = ods.Draw,
                LoseOdds = ods.Lose,
                LeagueId = leagueId,
                Winner = loadedMatch.Score.Winner switch {
                    "HOME_TEAM" => MatchWinner.HomeTeam,
                    "AWAY_TEAM" => MatchWinner.AwayTeam,
                    _ => MatchWinner.Draw
                },
                FullTimeHomeScore = loadedMatch.Score.FullTime.Home,
                FullTimeAwayScore = loadedMatch.Score.FullTime.Away,
                HalfTimeHomeScore = loadedMatch.Score.HalfTime?.Home,
                HalfTimeAwayScore = loadedMatch.Score.HalfTime?.Away,
                Country = loadedMatch.Area?.Name
            };

            _context.Matches.Add(match);
        }

        return;

        Guid GetTeamId(TeamModel team)
        {
            var teamId = teams.FirstOrDefault(x => x.ExternalId == team.Id)?.Id;
            if (teamId.HasValue)
            {
                return teamId.Value;
            }

            var homeTeam = new Team
            {
                Id = Guid.NewGuid(),
                Name = team.Name,
                ShortName = team.ShortName,
                Tla = team.Tla,
                CrestUrl = team.Crest
            };
            _context.Teams.Add(homeTeam);

            return homeTeam.Id;
        }
    }
}