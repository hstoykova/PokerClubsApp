using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Game;
using PokerClubsApp.Web.ViewModels.GameResults;
using System.Globalization;
using static PokerClubsApp.Common.EntityValidationConstants.GameResult;

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

        [HttpPost]
        public async Task<IActionResult> Add(AddGameResultsModel model)
        {
            if (ModelState.IsValid == false)
            {
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlaters();

                return View(model);
            }

            DateTime fromDate;
            if (DateTime.TryParseExact(model.FromDate, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDate) == false)
            {
                ModelState.AddModelError(nameof(model.FromDate), "Invalid date format");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlaters();

                return View(model);
            }

            DateTime toDate;
            if (DateTime.TryParseExact(model.ToDate, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out toDate) == false)
            {
                ModelState.AddModelError(nameof(model.ToDate), "Invalid date format");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlaters();

                return View(model);
            }

            if (fromDate.DayOfWeek != DayOfWeek.Monday || fromDate > toDate)
            {
                ModelState.AddModelError(nameof(model.FromDate), "From date must be Monday and earlier than To date!");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlaters();

                return View(model);
            }

            if (toDate.DayOfWeek != DayOfWeek.Sunday || toDate < fromDate)
            {
                ModelState.AddModelError(nameof(model.ToDate), "To date must be Sunday and later than From date!");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlaters();

                return View(model);
            }

            var player = await context.Players
                .Where(p => p.AccountId == model.AccountId)
                .Include(p => p.Memberships)
                .FirstOrDefaultAsync();

            // TODO If player is new - add button for adding new player

            //if (player is null)
            //{

            //}

            var membership = await context.Memberships
                .Where(m => m.ClubId == model.ClubId && m.PlayerAccountId == model.AccountId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (membership == null)
            {
                membership = new Membership()
                {
                    Player = player!,
                    ClubId = model.ClubId
                };

                await context.Memberships.AddAsync(membership);
            }

            PlayerGame playerGame = new PlayerGame()
            {
                Membership = membership!,
                GameTypeId = model.GameTypeId,
                FromDate = fromDate,
                ToDate = toDate,
                Result = model.Result,
                Fee = model.Fee
            };

            await context.PlayersGames.AddAsync(playerGame);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = playerGame.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await context.PlayersGames
                .Where(pg => pg.Id == id)
                .AsNoTracking()
                .Select(pg => new GameResultDetailsModel()
                {
                    UnionName = pg.Membership.Club.Union.Name,
                    PlayerAccountId = pg.Membership.PlayerAccountId,
                    Nickname = pg.Membership.Player.Nickname,
                    ClubName = pg.Membership.Club.Name,
                    Result = pg.Result,
                    Fee = pg.Fee,
                    FromDate = pg.FromDate,
                    ToDate = pg.ToDate,
                    GameType = pg.GameType.Name
                })
                .FirstOrDefaultAsync();

            return View(model);
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
