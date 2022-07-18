using FootballData.Models;
using Microsoft.EntityFrameworkCore;


namespace FootballData
{
    public class FootballDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<PlayersInTeams> PlayersInTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Player>()
                .HasMany(p => p.Teams)
                .WithMany(t => t.Players)
                .UsingEntity<PlayersInTeams>(
                j => j
                    .HasOne(pt => pt.Team)
                    .WithMany(t => t.PlayersInTeams)
                    .HasForeignKey(pt => pt.TeamId),    // связь с таблицей Teams через TeamId
                j => j
                    .HasOne(pt => pt.Player)
                    .WithMany(p => p.PlayersInTeams)
                    .HasForeignKey(pt => pt.PlayerId),  // связь с таблицей Players через PlayerId
                j => {
                    j.HasKey(pt => new { pt.PlayerId, pt.TeamId });
                });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FootballDb;Username=postgres;Password=12345");
        }
    }
}
