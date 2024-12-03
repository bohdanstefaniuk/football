using Football.Entities.Enums;

namespace Football.Services.Matches.Models;

public class LeagueModel
{
    public string Name { get; set; }
    public string Country { get; set; }
    public IEnumerable<MatchModel> Matches { get; set; }

    public class MatchModel
    {
        public DateTime Date { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamCrestUrl { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamCrestUrl { get; set; }

        public MatchWinner? Winner { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }

        public double WinOdds { get; set; }
        public double DrawOdds { get; set; }
        public double LoseOdds { get; set; }

        public string Country { get; set; }
    }

}