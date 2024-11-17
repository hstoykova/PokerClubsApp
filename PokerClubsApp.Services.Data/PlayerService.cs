using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PokerClubsApp.Services.Data
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository<Player, int> playerRepository;

        public PlayerService(IRepository<Player, int> playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await playerRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .ToListAsync();
        }
       
    }
}
