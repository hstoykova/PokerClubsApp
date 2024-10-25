using PokerClubsApp.Data.Models;
using Microsoft.EntityFrameworkCore;


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

        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Union> Unions { get; set; }
        public virtual DbSet<GameType> GameTypes { get; set; }
        public virtual DbSet<PlayedGame> PlayedGames { get; set; }
        public virtual DbSet<PlayerGame> PlayersGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerGame>()
                .HasKey(pg => new { pg.PlayerId, pg.GameId });
        }
    }
}
