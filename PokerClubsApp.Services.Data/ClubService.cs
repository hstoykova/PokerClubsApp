using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;

namespace PokerClubsApp.Services.Data
{
    public class ClubService : IClubService
    {
        private readonly IRepository<Club, int> clubRepository;
        public ClubService(IRepository<Club, int> clubRepository)
        {
            this.clubRepository = clubRepository;
        }

        public async Task<IEnumerable<Club>> GetAllClubsAsync()
        {
            return await clubRepository
                .GetAllAttached()
                .Where(c => c.IsDeleted == false)
                .ToListAsync();
        }
    }
}
