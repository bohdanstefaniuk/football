using System.Net;
using Football.ExternalServices.Clients.Models;
using Football.ExternalServices.Clients.Responses;
using Football.ExternalServices.Interfaces;

namespace Football.ExternalServices.Clients;

internal class FootballDataClient : IFootballDataClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FootballDataClient> _logger;

    public FootballDataClient(IHttpClientFactory httpClientFactory, ILogger<FootballDataClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<CompetitionModel>> GetCompetitions()
    {
        var response = await Get("competitions");
        if (response == null)
        {
            return [];
        }

        var content = await response.Content.ReadFromJsonAsync<CompetitionsResponse>();
        return content != null ? content.Competitions : [];
    }

    public async Task<IEnumerable<TeamModel>> GetTeams(string competitionCode)
    {
        var response = await Get($"competitions/{competitionCode}/teams");
        if (response == null)
        {
            return [];
        }

        var content = await response.Content.ReadFromJsonAsync<TeamsResponse>();
        return content != null ? content.Teams : [];
    }

    public async Task<IEnumerable<MatchModel>> GetMatches(string competitionCode, DateTime from, DateTime to)
    {
        var response = await Get($"competitions/{competitionCode}/matches?dateFrom={from:yyyy-MM-dd}&dateTo={to:yyyy-MM-dd}");
        if (response == null)
        {
            return [];
        }

        var content = await response.Content.ReadFromJsonAsync<MatchesResponse>();
        return content != null ? content.Matches : [];
    }

    public async Task<MatchModel> GetMatch(int matchId)
    {
        var response = await Get($"matches/{matchId}");
        if (response == null)
        {
            return null;
        }

        var content = await response.Content.ReadFromJsonAsync<MatchModel>();
        return content;
    }

    private async Task<HttpResponseMessage> Get(string url)
    {
        try
        {
            var client = _httpClientFactory.CreateClient(ExternalServicesConstants.FootballDataClientName);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException e)
        {
            // I assume that the 404 status code can be reported as null
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning(e, "Resource not found: {Url}", url);
                return null;
            }

            // We can log the exception details here
            // And throw a custom exception to hide the implementation details
            _logger.LogError(e, "Failed to get resource {Url} with status code {StatusCode}", url, e.StatusCode);
            throw;
        }
    }
}