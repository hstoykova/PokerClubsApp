﻿using PokerClubsApp.Data.Models;
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
        public virtual DbSet<PlayedGame> PlayedGames { get; set; } = null!;
        public virtual DbSet<PlayerGame> PlayersGames { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerGame>()
                .HasKey(pg => new { pg.PlayerAccountId, pg.GameId });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
