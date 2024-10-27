
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerClubsApp.Data.Models;

namespace PokerClubsApp.Data.Configuration
{
    public class UnionConfiguration : IEntityTypeConfiguration<Union>
    {
        public void Configure(EntityTypeBuilder<Union> builder)
        {
            builder.HasData(this.SeedUnions());
        }

        private List<Union> SeedUnions()
        {
            return new List<Union>()
            {
                new Union { Id = 1, Name = "Orange union"},
                new Union { Id = 2, Name = "Sunshine union"},
                new Union { Id = 3, Name = "ComeToWin"}
            };
        }
    }
}
