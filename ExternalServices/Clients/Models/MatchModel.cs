namespace Football.ExternalServices.Clients.Models;

public class MatchModel
{
    public int Id { get; set; }
    public AreaModel Area { get; set; }
    public DateTime UtcDate { get; set; }
    public string Status { get; set; }
    public DateTime LastUpdated { get; set; }
    public TeamModel HomeTeam { get; set; }
    public TeamModel AwayTeam { get; set; }
    public ScoreModel Score { get; set; }
}