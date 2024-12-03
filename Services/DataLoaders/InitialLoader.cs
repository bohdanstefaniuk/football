using Football.Data;
using Microsoft.EntityFrameworkCore;

namespace Football.Services.DataLoaders;

public class InitialLoader
{
    private readonly DatabaseContext _context;
    private readonly LeaguesLoader _leaguesLoader;
    private readonly TeamsLoader _teamsLoader;
    private readonly MatchesLoader _matchesLoader;

    public InitialLoader(
        DatabaseContext context,
        LeaguesLoader leaguesLoader,
        TeamsLoader teamsLoader,
        MatchesLoader matchesLoader)
    {
        _context = context;
        _leaguesLoader = leaguesLoader;
        _teamsLoader = teamsLoader;
        _matchesLoader = matchesLoader;
    }

    public async Task LoadAsync()
    {
        if (!await _context.Leagues.AnyAsync())
        {
            await _leaguesLoader.LoadAsync();
            await Task.Delay(60_000);
        }

        if (!await _context.Teams.AnyAsync())
        {
            await _teamsLoader.LoadAsync();
            await Task.Delay(60_000);
        }

        if (!await _context.Matches.AnyAsync())
        {
            await _matchesLoader.LoadUpcomingAsync();
            await Task.Delay(60_000);
        }
    }
}