using Football.Services.Odds.Models;

namespace Football.Services.Odds.Interfaces;

public interface IOddsService
{
    Task<OddsModel> GetMatchOdds();
}