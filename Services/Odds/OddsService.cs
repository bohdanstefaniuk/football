using Football.Services.Odds.Interfaces;
using Football.Services.Odds.Models;

namespace Football.Services.Odds;

public class OddsService : IOddsService
{
    private static readonly Random Random = new();

    // Instead of calling an external API, we will generate random odds (for demo purposes)
    // I made this method async to simulate a real world scenario where we would call an external API
    public async Task<OddsModel> GetMatchOdds()
    {
        return await Task.FromResult(new OddsModel
        {
            Win = Random.NextDouble() * 10,
            Draw = Random.NextDouble() * 10,
            Lose = Random.NextDouble() * 10
        });
    }
}