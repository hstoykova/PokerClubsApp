using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PokerClubsApp.Data
{
    public class PokerClubsDbContext : IdentityDbContext
    {
        public PokerClubsDbContext(DbContextOptions<PokerClubsDbContext> options)
            : base(options)
        {
        }
    }
}
