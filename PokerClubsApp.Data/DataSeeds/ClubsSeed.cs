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
    public class ClubsSeed : IEntityTypeConfiguration<Club>
    {

        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.HasData(this.SeedClubs());
        }

        private List<Club> SeedClubs()
        {
            return new List<Club>()
            {
                new Club { Id = 1, Name = "GreenLine", UnionId = 1},
                new Club { Id = 2, Name = "Obi-One", UnionId = 2},
                new Club { Id = 3, Name = "Galaxy", UnionId = 3}
            };
        }
    }
}
