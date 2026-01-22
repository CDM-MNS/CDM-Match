using CDM.Match.DTO;
using CDM.Match.Repository;
using CMD.Match.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace CDM.Match.Controller;

public record Pingv2(string CorrelationId, string Message);

[Route("api/[controller]")]
[ApiController]
public class MatchController(IMatchRepository matchRepository, IRabbitMQService mq) : ControllerBase
{

    [HttpGet("{id}")]
    public IActionResult GetMatchById(int id)
    {
        var match = matchRepository.GetMatchById(id);
        mq.PublishAsync("get-odds", id, "odds.get");
        return Ok(match);
    }

    [HttpPost("test")]
    public IActionResult PublishTest()
    {
        Pingv2 pong = new Pingv2(Guid.NewGuid().ToString(), "testpong");
        mq.PublishAsync("pong-queue", pong, "test.event");
        Console.WriteLine("Message published " +  nameof(Pingv2));
        return Ok("Message published");
    }

    [HttpPost("odds")]
    public IActionResult GetOdds([FromBody] GetOddsRequestDTO odds)
    {
        mq.PublishAsync("get-odds-in", odds, "odds.get");
        return Ok("Message published");
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