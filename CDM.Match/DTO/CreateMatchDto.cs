namespace CDM.Match.DTO;

public class CreateMatchDto
{
    public string FirstTeam { get; set; }
    public string SecondTeam { get; set; }
    public DateOnly CreatedAt { get; set; }
    public DateOnly MatchDate { get; set; }
}