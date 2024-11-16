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
            model.Players = await GetAllPlayers();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddGameResultsModel model)
        {
            if (ModelState.IsValid == false)
            {
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlayers();

                return View(model);
            }

            DateTime fromDate;
            if (DateTime.TryParseExact(model.FromDate, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDate) == false)
            {
                ModelState.AddModelError(nameof(model.FromDate), "Invalid date format");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlayers();

                return View(model);
            }

            DateTime toDate;
            if (DateTime.TryParseExact(model.ToDate, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out toDate) == false)
            {
                ModelState.AddModelError(nameof(model.ToDate), "Invalid date format");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlayers();

                return View(model);
            }

            if (fromDate.DayOfWeek != DayOfWeek.Monday || fromDate > toDate)
            {
                ModelState.AddModelError(nameof(model.FromDate), "From date must be Monday and earlier than To date!");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlayers();

                return View(model);
            }

            if (toDate.DayOfWeek != DayOfWeek.Sunday || toDate < fromDate)
            {
                ModelState.AddModelError(nameof(model.ToDate), "To date must be Sunday and later than From date!");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlayers();

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
                .Where(pg => pg.IsDeleted == false)
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.PlayersGames
                .Where(pg => pg.Id == id)
                .Where(pg => pg.IsDeleted == false)
                .AsNoTracking()
                .Select(pg => new EditGameResultsModel()
                {
                    Nickname = pg.Membership.Player.Nickname,
                    ClubId = pg.Membership.ClubId,
                    Result = pg.Result,
                    Fee = pg.Fee,
                    FromDate = pg.FromDate.ToString(FromDateFormat),
                    ToDate = pg.ToDate.ToString(ToDateFormat),
                    GameTypeId = pg.GameTypeId
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            model.GameTypes = await GetAllGameTypes();
            model.Clubs = await GetAllClubs();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditGameResultsModel model, int id)
        {
            if (ModelState.IsValid == false)
            {
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();

                return View(model);
            }

            DateTime fromDate;
            if (DateTime.TryParseExact(model.FromDate, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDate) == false)
            {
                ModelState.AddModelError(nameof(model.FromDate), "Invalid date format");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();

                return View(model);
            }

            DateTime toDate;
            if (DateTime.TryParseExact(model.ToDate, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out toDate) == false)
            {
                ModelState.AddModelError(nameof(model.ToDate), "Invalid date format");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();

                return View(model);
            }

            if (fromDate.DayOfWeek != DayOfWeek.Monday || fromDate > toDate)
            {
                ModelState.AddModelError(nameof(model.FromDate), "From date must be Monday and earlier than To date!");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();

                return View(model);
            }

            if (toDate.DayOfWeek != DayOfWeek.Sunday || toDate < fromDate)
            {
                ModelState.AddModelError(nameof(model.ToDate), "To date must be Sunday and later than From date!");
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();

                return View(model);
            }

            PlayerGame? playerGame = await context.PlayersGames.FindAsync(id);

            if (playerGame == null || playerGame.IsDeleted)
            {
                return NotFound();
            }

            var player = await context.Memberships
                .Where(m => m.Id == playerGame.MembershipId)
                .AsNoTracking()
                .Select(m => m.Player)
                .FirstOrDefaultAsync();

            var membership = await context.Memberships
                .Where(m => m.ClubId == model.ClubId && m.PlayerAccountId == player!.AccountId)
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

            playerGame.Membership = membership;
            playerGame.GameTypeId = model.GameTypeId;
            playerGame.FromDate = fromDate;
            playerGame.ToDate = toDate;
            playerGame.Result = model.Result;
            playerGame.Fee = model.Fee;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = playerGame.Id });
        }

        private async Task<List<GameType>> GetAllGameTypes()
        {
            return await context.GamesTypes.ToListAsync();
        }

        private async Task<List<Club>> GetAllClubs()
        {
            return await context.Clubs
                .Where(c => c.IsDeleted == false)
                .ToListAsync();
        }

        private async Task<List<Player>> GetAllPlayers()
        {
            return await context.Players
                .Where(p => p.IsDeleted == false)
                .ToListAsync();
        }
    }
}
