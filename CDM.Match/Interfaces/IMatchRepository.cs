using CDM.Match.DTO;
using CDM.Match.Models;

namespace CDM.Match.Repository;

public interface IMatchRepository
{
    MatchEntity GetMatchById(int id);
    List<MatchEntity> GetAllMatches();
    Task AddMatch(CreateMatchDto match);
    Task UpdateMatch(UpdateMatchDateDto matchDateDto, int id);
    Task DeleteMatch(int id);
}