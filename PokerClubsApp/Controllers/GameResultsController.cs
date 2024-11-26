﻿using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IPlayerService playerService;
        private readonly IClubService clubService;
        private readonly IGameTypeService gameTypeService;

        public GameResultsController(PokerClubsDbContext _context, IGameResultService gameResultsService, IPlayerService playerService, 
            IClubService clubService, IGameTypeService gameTypeService)
        {
            context = _context;
            this.gameResultsService = gameResultsService;
            this.playerService = playerService;
            this.clubService = clubService;
            this.gameTypeService = gameTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AddGameResultsModel();

            var week = new Week(DateTime.Now);
            var weeks = Enumerable.Range(1, 3)
                .Select(i => week.AddWeeks(-i))
                .ToList();

            model.GameTypes = (await gameTypeService.GetAllGameTypesAsync()).ToList();
            model.Clubs = (await clubService.GetAllClubsAsync()).ToList();
            model.Players = (await playerService.GetAllPlayersAsync()).ToList();
            model.Weeks = weeks;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddGameResultsModel model)
        {
            if (ModelState.IsValid == false)
            {
                model.GameTypes = (await gameTypeService.GetAllGameTypesAsync()).ToList();
                model.Clubs = (await clubService.GetAllClubsAsync()).ToList();
                model.Players = (await playerService.GetAllPlayersAsync()).ToList();

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
                model.GameTypes = (await gameTypeService.GetAllGameTypesAsync()).ToList();
                model.Clubs = (await clubService.GetAllClubsAsync()).ToList();
                model.Players = (await playerService.GetAllPlayersAsync()).ToList();

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
                    PlayerId = pg.Membership.PlayerId,
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
        [Authorize(Roles = "Admin")]
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

            var selectedWeek = $"{model.FromDate} - {model.ToDate}";

            var week = new Week(DateTime.Now);
            var weeks = Enumerable.Range(1, 3)
                .Select(i => week.AddWeeks(-i))
                .ToList();

            model.GameTypes = (await gameTypeService.GetAllGameTypesAsync()).ToList();
            model.Clubs = (await clubService.GetAllClubsAsync()).ToList();
            model.Weeks = weeks;
            model.Week = selectedWeek;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditGameResultsModel model, int id)
        {
            if (ModelState.IsValid == false)
            {
                model.GameTypes = (await gameTypeService.GetAllGameTypesAsync()).ToList();
                model.Clubs = (await clubService.GetAllClubsAsync()).ToList();

                return View(model);
            }

            var gameResult = await gameResultsService.EditGameResultAsync(model, id);

            // TODO Check if gameResult is null
            //if (gameResult == null) 
            //{

            //}

            return RedirectToAction(nameof(Details), new { id = gameResult.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await gameResultsService.DeleteGameResultAsync(id);

            return RedirectToAction("Index", "Home");
        }
    }
}
