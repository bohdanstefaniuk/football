using Football.Data;
using Football.Entities;
using Football.ExternalServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Football.Services.DataLoaders;

public class LeaguesLoader
{
    private readonly DatabaseContext _context;
    private readonly IFootballDataClient _footballDataClient;
    private readonly ILogger<LeaguesLoader> _logger;

    public LeaguesLoader(
        DatabaseContext context,
        IFootballDataClient footballDataClient,
        ILogger<LeaguesLoader> logger)
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

        var leaguesToLoad = Constants.SupportedLeagues.Except(leagues).ToArray();
        if (leaguesToLoad.Length == 0)
        {
            return;
        }

        try
        {
            var loadedLeagues = await _footballDataClient.GetCompetitions();
            loadedLeagues = loadedLeagues.Where(x => leaguesToLoad.Contains(x.Code)).ToArray();

            foreach (var loadedLeague in loadedLeagues)
            {
                var league = new League
                {
                    Id = Guid.NewGuid(),
                    Name = loadedLeague.Name,
                    Code = loadedLeague.Code,
                    EmblemUrl = loadedLeague.Emblem,
                    Country = loadedLeague.Area?.Name
                };

                _context.Leagues.Add(league);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to load leagues");
        }
    }
}