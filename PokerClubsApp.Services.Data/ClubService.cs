using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Clubs;

namespace PokerClubsApp.Services.Data
{
    public class ClubService : IClubService
    {
        private readonly IRepository<Club, int> clubRepository;
        public ClubService(IRepository<Club, int> clubRepository)
        {
            this.clubRepository = clubRepository;
        }

        public async Task<Club> CreateClubAsync(AddClubModel model)
        {
            Club club = new Club() 
            {
                Name = model.Name,
                UnionId = model.UnionId
            };

            await clubRepository.AddAsync(club);

            return club;
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
