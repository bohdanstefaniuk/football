using Football.Entities.Enums;

namespace Football.Entities;

public class Match
{
    public Guid Id { get; set; }
    public int ExternalId { get; set; }
    public Guid LeagueId { get; set; }
    public DateTime Date { get; set; }
    public MatchStatus Status { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }

    // I prefer to use nullable types for scores,
    // because the match may not have started yet and 0 is invalid in this case
    public MatchWinner? Winner { get; set; }
    public int? FullTimeHomeScore { get; set; }
    public int? FullTimeAwayScore { get; set; }
    public int? HalfTimeHomeScore { get; set; }
    public int? HalfTimeAwayScore { get; set; }
    public bool IsDeleted { get; set; }

    public double WinOdds { get; set; }
    public double DrawOdds { get; set; }
    public double LoseOdds { get; set; }

    public string Country { get; set; }

    public League League { get; set; }
    public Team HomeTeam { get; set; }
    public Team AwayTeam { get; set; }
}