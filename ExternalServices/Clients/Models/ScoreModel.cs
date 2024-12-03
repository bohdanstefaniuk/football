namespace Football.ExternalServices.Clients.Models;

public class ScoreModel
{
    public string Winner { get; set; }
    public string Duration { get; set; }
    public ScoreDetailModel FullTime { get; set; }
    public ScoreDetailModel HalfTime { get; set; }

    public class ScoreDetailModel
    {
        public int? Home { get; set; }
        public int? Away { get; set; }
    }
}