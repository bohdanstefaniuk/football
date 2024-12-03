using Football.Services.Matches;
using Football.Services.Matches.Models;
using Microsoft.AspNetCore.Mvc;

namespace Football.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchesController: ControllerBase
{
    [HttpGet("recent")]
    public async Task<IEnumerable<LeagueModel>> GetRecentMatches([FromServices] MatchesService matchesService)
    {
        return await matchesService.GetRecentMatches();
    }

    [HttpGet("upcoming")]
    public async Task<IEnumerable<LeagueModel>> GetUpcomingMatches([FromServices] MatchesService matchesService)
    {
        return await matchesService.GetUpcomingMatches();
    }
}