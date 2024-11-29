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

        public async Task<Club> CreateClubAsync(CreateClubModel model)
        {
            Club club = new Club() 
            {
                Name = model.Name,
                UnionId = model.UnionId
            };

            await clubRepository.AddAsync(club);

            return club;
        }

        public async Task<CreateClubModel?> GetClubForEditAsync(int id)
        {
            var club = await clubRepository.GetByIdAsync(id);

            if (club?.IsDeleted ?? true)
            {
                return null;
            }

            var model = new CreateClubModel() 
            {
                Name = club!.Name,
                UnionId = club.UnionId
            };

            return model;
        }

        public async Task<Club?> EditClubAsync(CreateClubModel model, int id)
        {
            var club = await clubRepository.GetByIdAsync(id);

            if (club?.IsDeleted ?? true)
            {
                return null;
            }

            club!.Name = model.Name;
            club.UnionId = model.UnionId;

            await clubRepository.UpdateAsync(club);

            return club;
        }

        

        public async Task<IEnumerable<Club>> GetAllClubsAsync()
        {
            return await clubRepository
                .GetAllAttached()
                .Where(c => c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Club>> IndexGetAllClubsAsync()
        {
            return await clubRepository.GetAllAttached()
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Union)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Club?> GetClubDetailsAsync(int id)
        {
            return await clubRepository.GetAllAttached()
                .Where(c => c.Id == id)
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Union)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
