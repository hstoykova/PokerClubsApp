﻿using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Common;
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
		private readonly IGameResultService gameResultsService;
		private readonly IPlayerService playerService;
		private readonly IClubService clubService;
		private readonly IGameTypeService gameTypeService;

		public GameResultsController(IGameResultService gameResultsService, IPlayerService playerService,
			IClubService clubService, IGameTypeService gameTypeService)
		{
			this.gameResultsService = gameResultsService;
			this.playerService = playerService;
			this.clubService = clubService;
			this.gameTypeService = gameTypeService;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			var model = new CreateGameResultsModel();

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
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(CreateGameResultsModel model)
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
				var gameResultId = await gameResultsService.CreateGameResultAsync(model);

				return RedirectToAction(nameof(Details), new
				{
					id = gameResultId
				});
			}
			catch (ValidationException e)
			{
				ModelState.AddModelError(e.Field, e.Message);
				model.GameTypes = (await gameTypeService.GetAllGameTypesAsync()).ToList();
				model.Clubs = (await clubService.GetAllClubsAsync()).ToList();
				model.Players = (await playerService.GetAllPlayersAsync()).ToList();

				return View(model);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Details(int id)
		{
			var model = await gameResultsService.GetGameResultsDetailsAsync(id);

			if (model == null)
			{
				return NotFound();
			}

			return View(model);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await gameResultsService.GetGameResultForEditAsync(id);

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

			try
			{
				var gameResult = await gameResultsService.EditGameResultAsync(model, id);

				if (gameResult == null)
				{
					return NotFound();
				}
				return RedirectToAction(nameof(Details), new { id = gameResult.Id });
			}
			catch (ValidationException e)
			{
				ModelState.AddModelError(e.Field, e.Message);

				model.GameTypes = (await gameTypeService.GetAllGameTypesAsync()).ToList();
				model.Clubs = (await clubService.GetAllClubsAsync()).ToList();

				return View(model);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			await gameResultsService.DeleteGameResultAsync(id);

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Index(string? Week, int? ClubId, int PageNumber = 1, int PageSize = 5)
		{
			Week? parsedWeek = null;

			if (Week != null)
			{
				var fromDate = Week.Split(" - ").FirstOrDefault();
				DateTime date = DateTime.ParseExact(fromDate, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None);
				parsedWeek = new Week(date);
			}

			(IndexGameResultsModel, int) result;

			if (User.IsInRole("Player"))
			{
				result = await gameResultsService.IndexGetAllGameResultsAsync(User.Identity!.Name!, parsedWeek, ClubId, PageNumber, PageSize);
			}
			else
			{
				result = await gameResultsService.IndexGetAllGameResultsAsync(parsedWeek, ClubId, PageNumber, PageSize);
			}

            ViewData["CurrentPage"] = PageNumber;
            ViewData["TotalPages"] = result.Item2;

            return View(result.Item1);

        }
	}
}
