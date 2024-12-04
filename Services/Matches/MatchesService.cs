using System.Linq.Expressions;
using Football.Data;
using Football.Entities;
using Football.Entities.Enums;
using Football.Services.Matches.Models;
using Microsoft.EntityFrameworkCore;

namespace Football.Services.Matches;

public class MatchesService
{
    private readonly DatabaseContext _context;

    public MatchesService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LeagueModel>> GetUpcomingMatches()
    {
        return await GetMatches(
            predicate: x => x.Status == MatchStatus.Scheduled && x.Date > DateTime.UtcNow,
            descending: false);
    }

    public async Task<IEnumerable<LeagueModel>> GetRecentMatches()
    {
        return await GetMatches(
            predicate: x => x.Status == MatchStatus.Finished && x.Date <= DateTime.UtcNow,
            descending: true);
    }

    // POSSIBLE IMPROVEMENT:
    // - Add pagination for better user experience
    // - Use "select" loading to load only necessary data
    private async Task<IEnumerable<LeagueModel>> GetMatches(Expression<Func<Match, bool>> predicate, bool descending = true)
    {
        var query = _context.Matches
            .Include(x => x.League)
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .Where(predicate);

        query = descending
            ? query.OrderByDescending(x => x.Date)
            : query.OrderBy(x => x.Date);

        var matches = await query
            .Skip(0)
            .Take(Constants.MatchesCountPerPage)
            .ToArrayAsync();

        return matches
            .GroupBy(x => x.LeagueId)
            .Select(x => new LeagueModel
            {
                Name = x.First().League.Name,
                Country = x.First().League.Country,
                Matches = x.Select(m => new LeagueModel.MatchModel
                {
                    Date = m.Date,
                    HomeTeamName = m.HomeTeam.Name,
                    HomeTeamCrestUrl = m.HomeTeam.CrestUrl,
                    AwayTeamName = m.AwayTeam.Name,
                    AwayTeamCrestUrl = m.AwayTeam.CrestUrl,
                    Winner = m.Winner,
                    HomeTeamScore = m.FullTimeHomeScore + m.HalfTimeHomeScore,
                    AwayTeamScore = m.FullTimeAwayScore + m.HalfTimeAwayScore,
                    WinOdds = Math.Round(m.WinOdds, 2),
                    DrawOdds = Math.Round(m.DrawOdds, 2),
                    LoseOdds = Math.Round(m.LoseOdds, 2),
                    Country = m.Country
                })
            })
            .ToArray();
    }
}