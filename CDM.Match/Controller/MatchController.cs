using CDM.Match.DTO;
using CDM.Match.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CDM.Match.Controller;

 
[Route("api/[controller]")]
[ApiController]
public class MatchController(IMatchRepository matchRepository) : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetMatchById(int id)
    {
        var match = matchRepository.GetMatchById(id);
        return Ok(match);
    }
    
    [HttpGet("all")]
    public IActionResult GetAllMatches()
    {
        var matches = matchRepository.GetAllMatches();
        return Ok(matches);
    }

    [HttpPost]
    public async Task<IActionResult> AddMatch(CreateMatchDto match)
    {
        await matchRepository.AddMatch(match);
        return Ok("Match added");
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateMatchFinised(UpdateMatchDateDto matchDateDto, int id)
    {
        await matchRepository.UpdateMatch(matchDateDto, id);
        return Ok($"Match {id} updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatch(int id)
    {
        await matchRepository.DeleteMatch(id);
        return Ok($"Match {id} deleted");
    }
    
}