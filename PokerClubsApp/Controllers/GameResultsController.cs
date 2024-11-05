using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Game;

namespace PokerClubsApp.Controllers
{
    public class GameResultsController : Controller
    {
        private readonly PokerClubsDbContext context;

        public GameResultsController(PokerClubsDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddGameResultsModel();
            model.GameTypes = await GetAllGameTypes();
            model.Clubs = await GetAllClubs();
            model.Players = await GetAllPlaters();

            return this.View(model);
        }

        private async Task<List<GameType>> GetAllGameTypes()
        {
            return await context.GamesTypes.ToListAsync();
        }

        private async Task<List<Club>> GetAllClubs()
        {
            return await context.Clubs.ToListAsync();
        }

        private async Task<List<Player>> GetAllPlaters()
        {
            return await context.Players.ToListAsync();
        }
    }
}
