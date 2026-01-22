using CDM.Match.DTO;
using CDM.Match.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;

namespace CDM.Match.Repository;

public class MatchRepository(MatchDbContext context) : IMatchRepository
{
    public MatchEntity GetMatchById(int id)
    {
        var Match = context.Matches.Find(id);
        return Match;
    }

    public List<MatchEntity> GetAllMatches()
    {
        return context.Matches.ToList();
    }
    
    
    public async Task AddMatch(CreateMatchDto match)
    {
        MatchEntity entity = new MatchEntity()
        {
            FirstTeam = match.FirstTeam,
            SecondTeam = match.SecondTeam,
            MatchDate = match.MatchDate,
        };
        
        await context.Matches.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateMatch(UpdateMatchDateDto matchDateDto, int id)
    {
        var Match = context.Matches.Find(id);
        Match.IsFinished = matchDateDto.IsFinised;
        await context.SaveChangesAsync();
    }

    public async Task DeleteMatch(int id)
    {
        var Match = context.Matches.Find(id);
        context.Matches.Remove(Match);
        await context.SaveChangesAsync();
    }
    
    
}

