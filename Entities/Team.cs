namespace Football.Entities;

public class Team
{
    public Guid Id { get; set; }
    public int ExternalId { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Tla { get; set; }
    public string CrestUrl { get; set; }
}