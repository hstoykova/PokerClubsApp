using PokerClubsApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace PokerClubsApp.Data
{
    public class PokerClubsDbContext : DbContext
    {
        public PokerClubsDbContext()
        {
            
        }
        public PokerClubsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Club> Clubs { get; set; } = null!;
        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<Union> Unions { get; set; } = null!;
        public virtual DbSet<GameType> GamesTypes { get; set; } = null!;
        public virtual DbSet<PlayedGame> PlayedGames { get; set; } = null!;
        public virtual DbSet<PlayerGame> PlayersGames { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerGame>()
                .HasKey(pg => new { pg.PlayerAccountId, pg.GameId });
        }
    }
}
