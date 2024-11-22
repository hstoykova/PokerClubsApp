using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerClubsApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Data.Configuration
{
    public class GameTypesSeed : IEntityTypeConfiguration<GameType>
    {
        public void Configure(EntityTypeBuilder<GameType> builder)
        {
            builder.HasData(this.SeedGameTypes());
        }

        private List<GameType> SeedGameTypes()
        {
            return new List<GameType>()
            {
                new GameType { Id = 1, Name = "PLO4"},
                new GameType { Id = 2, Name = "PLO5"},
                new GameType { Id = 3, Name = "PLO6"},
                new GameType { Id = 4, Name = "NLH"}
            };
        }
    }
}
