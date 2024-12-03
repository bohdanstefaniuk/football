using Football.Services.DataLoaders;
using Football.Services.Matches;
using Football.Services.Odds;
using Football.Services.Odds.Interfaces;

namespace Football.Services;

public static class DependencyLoader
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddTransient<IOddsService, OddsService>();
        services.AddTransient<LeaguesLoader>();
        services.AddTransient<TeamsLoader>();
        services.AddTransient<MatchesLoader>();
        services.AddTransient<InitialLoader>();
        services.AddTransient<MatchesService>();
    }
}