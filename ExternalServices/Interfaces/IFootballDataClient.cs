using Football.ExternalServices.Clients.Models;

namespace Football.ExternalServices.Interfaces;

public interface IFootballDataClient
{
    Task<IEnumerable<CompetitionModel>> GetCompetitions();
    Task<IEnumerable<TeamModel>> GetTeams(string competitionCode);
    Task<IEnumerable<MatchModel>> GetMatches(string competitionCode, DateTime from, DateTime to);
    Task<MatchModel> GetMatch(int matchId);
}