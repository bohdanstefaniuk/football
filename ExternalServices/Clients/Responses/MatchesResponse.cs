using Football.ExternalServices.Clients.Models;

namespace Football.ExternalServices.Clients.Responses;

internal class MatchesResponse
{
    public IEnumerable<MatchModel> Matches { get; set; }
}