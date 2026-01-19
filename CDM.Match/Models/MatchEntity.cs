namespace CDM.Match.Models;

public class MatchEntity
{
    public int MatchId { get; set; }
    public string FirstTeam { get; set; }
    public string SecondTeam { get; set; }
    public DateOnly CreatedAt { get; set; }
    public DateOnly MatchDate { get; set; }
    public bool IsFinished { get; set; }
}