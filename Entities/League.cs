namespace Football.Entities;

public class League
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string EmblemUrl { get; set; }
    public string Country { get; set; }

    public ICollection<Match> Matches { get; set; }
}