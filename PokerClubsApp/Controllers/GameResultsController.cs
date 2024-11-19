using Itenso.TimePeriod;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Game;
using PokerClubsApp.Web.ViewModels.GameResults;
using System.Globalization;
using static PokerClubsApp.Common.EntityValidationConstants.GameResult;

namespace PokerClubsApp.Controllers
{
    public class GameResultsController : Controller
    {
        private readonly PokerClubsDbContext context;
        private readonly IGameResultService gameResultsService;

        public GameResultsController(PokerClubsDbContext _context, IGameResultService gameResultsService)
        {
            context = _context;
            this.gameResultsService = gameResultsService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddGameResultsModel();

            var week = new Week(DateTime.Now);
            var weeks = Enumerable.Range(1, 3)
                .Select(i => week.AddWeeks(-i))
                .ToList();

            model.GameTypes = await GetAllGameTypes();
            model.Clubs = await GetAllClubs();
            model.Players = await GetAllPlayers();
            model.Weeks = weeks;

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

            try
            {
                var gameResultId = await gameResultsService.AddGameResultAsync(model);
                return RedirectToAction(nameof(Details), new
                {
                    id = gameResultId
                });
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(e.ParamName!, e.Message);
                model.GameTypes = await GetAllGameTypes();
                model.Clubs = await GetAllClubs();
                model.Players = await GetAllPlayers();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await context.GameResults
                .Where(pg => pg.Id == id)
                .Where(pg => pg.IsDeleted == false)
                .AsNoTracking()
                .Select(pg => new GameResultDetailsModel()
                {
                    Id = pg.Id,
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
            var model = await context.GameResults
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

            GameResult? playerGame = await context.GameResults.FindAsync(id);

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

        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.GameResults.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            model.IsDeleted = true;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
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
