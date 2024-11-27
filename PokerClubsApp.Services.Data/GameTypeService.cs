using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;

namespace PokerClubsApp.Services.Data
{
    public class GameTypeService : IGameTypeService
    {
        private readonly IRepository<GameType, int> gameTypeRepository;

        public GameTypeService(IRepository<GameType, int> gameTypeRepository)
        {
            this.gameTypeRepository = gameTypeRepository;
        }

        public async Task<IEnumerable<GameType>> GetAllGameTypesAsync()
        {
            return await gameTypeRepository.GetAllAsync();
        }

    }
}
