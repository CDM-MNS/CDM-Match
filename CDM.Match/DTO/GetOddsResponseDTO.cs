namespace CDM.Match.DTO;

public class GetOddsResponseDTO
{
    public int OddId { get; set; }
    public int MatchId { get; set; }
    public float OddFirstTeamWinning { get; set; }
    public float OddSecondTeamWinning { get; set; }
    public float OddDraw { get; set; }
    public DateOnly OddCreated { get; set; }
    public DateOnly OddUpdated { get; set; }
}