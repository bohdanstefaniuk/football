namespace Football.ExternalServices.Clients.Models;

public class CompetitionModel
{
    public int Id { get; set; }
    public AreaModel Area { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Emblem { get; set; }
}