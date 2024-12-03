using Football.Services.Matches;
using Football.Services.Matches.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Football.Pages;

public class UpcomingModel(MatchesService matchesService) : PageModel
{
    public IEnumerable<LeagueModel> Leagues { get; set; } = [];

    public async Task OnGet()
    {
        Leagues = await matchesService.GetUpcomingMatches();
    }
}