using CDM.Match.Models;
using Microsoft.EntityFrameworkCore;

namespace CDM.Match;

public class MatchDbContext : DbContext
{
    DbSet<MatchEntity> Matches { get; set; }
    
}