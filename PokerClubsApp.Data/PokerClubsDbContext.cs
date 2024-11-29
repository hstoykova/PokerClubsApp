using PokerClubsApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace PokerClubsApp.Data
{
    public class PokerClubsDbContext : IdentityDbContext<IdentityUser>
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
        public virtual DbSet<GameResult> GameResults { get; set; } = null!;
        public virtual DbSet<Membership> Memberships { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Membership>()
                .HasIndex(m => new { m.ClubId, m.PlayerId })
                .HasDatabaseName("IX_Memberships_Club_PlayerAccountId")
                .IsUnique();

            modelBuilder.Entity<Player>()
               .HasIndex(p => p.Nickname)
               .IsUnique();

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.AccountId)
                .IsUnique();

            modelBuilder.Entity<Player>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<Union>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<Club>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
