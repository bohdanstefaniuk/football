using Football.ExternalServices.Clients.Models;

namespace Football.ExternalServices.Clients.Responses;

internal class TeamsResponse
{
    public IEnumerable<TeamModel> Teams { get; set; }
}