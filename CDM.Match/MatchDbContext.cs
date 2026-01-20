using CDM.Match.Models;
using Microsoft.EntityFrameworkCore;

namespace CDM.Match;

public class MatchDbContext : DbContext
{
    public DbSet<MatchEntity> Matches { get; set; }
    
    public MatchDbContext(DbContextOptions<MatchDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MatchEntity>()
            .HasKey(m => m.MatchId);
    }
}