using Football.ExternalServices.Clients.Models;

namespace Football.ExternalServices.Clients.Responses;

internal class CompetitionsResponse
{
    public IEnumerable<CompetitionModel> Competitions { get; set; }
}