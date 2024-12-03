using Football.ExternalServices.Clients;
using Football.ExternalServices.Interfaces;
using Polly;

namespace Football.ExternalServices;

public static class DependencyLoader
{
    public static void AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        var externalServicesOptions = new ExternalServicesOptions();
        configuration.GetSection(ExternalServicesOptions.Title).Bind(externalServicesOptions);

        services.AddTransient<IFootballDataClient, FootballDataClient>();
        services
            .AddHttpClient(
                ExternalServicesConstants.FootballDataClientName,
                client =>
                {
                    client.BaseAddress = new Uri(externalServicesOptions.FootballDataBaseUrl);
                    client.DefaultRequestHeaders.Add("X-Auth-Token", externalServicesOptions.FootballDataApiKey);
                })
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500))
            );
    }
}