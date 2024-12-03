using Football.Data;
using Football.Entities;
using Football.ExternalServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Football.Services.DataLoaders;

public class TeamsLoader
{
    private readonly DatabaseContext _context;
    private readonly IFootballDataClient _footballDataClient;
    private readonly ILogger<TeamsLoader> _logger;

    public TeamsLoader(
        DatabaseContext context,
        IFootballDataClient footballDataClient,
        ILogger<TeamsLoader> logger)
    {
        _context = context;
        _footballDataClient = footballDataClient;
        _logger = logger;
    }

    public async Task LoadAsync()
    {
        var leagues = await _context.Leagues
            .Select(x => x.Code)
            .ToArrayAsync();

        var existingTeams = (await _context.Teams
            .Select(x => x.ExternalId)
            .ToArrayAsync()).ToHashSet();

        foreach (var league in leagues)
        {
            try
            {
                var loadedTeams = await _footballDataClient.GetTeams(league);
                loadedTeams = loadedTeams.Where(x => !existingTeams.Contains(x.Id)).ToArray();

                foreach (var loadedTeam in loadedTeams)
                {
                    var team = new Team
                    {
                        Id = Guid.NewGuid(),
                        ExternalId = loadedTeam.Id,
                        Name = loadedTeam.Name,
                        ShortName = loadedTeam.ShortName,
                        Tla = loadedTeam.Tla,
                        CrestUrl = loadedTeam.Crest
                    };

                    _context.Teams.Add(team);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load teams for league {LeagueCode}", league);
            }
        }

        await _context.SaveChangesAsync();
    }
}