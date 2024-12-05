using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Game;
using static PokerClubsApp.Common.EntityValidationConstants.GameResult;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Itenso.TimePeriod;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using PokerClubsApp.Web.ViewModels.GameResults;
using Microsoft.AspNetCore.Identity;

namespace PokerClubsApp.Services.Data
{
	public class GameResultService : IGameResultService
	{
		private readonly IRepository<Club, int> clubRepository;
		private readonly IRepository<Player, int> playerRepository;
		private readonly IRepository<GameType, int> gameTypeRepository;
		private readonly IRepository<Membership, int> membershipRepository;
		private readonly IRepository<GameResult, int> gameResultRepository;

		public GameResultService(IRepository<Club, int> clubRepository,
			IRepository<Player, int> playerRepository,
			IRepository<GameType, int> gameTypeRepository,
			IRepository<Membership, int> membershipRepository,
			IRepository<GameResult, int> gameResultRepository)
		{
			this.clubRepository = clubRepository;
			this.playerRepository = playerRepository;
			this.gameTypeRepository = gameTypeRepository;
			this.membershipRepository = membershipRepository;
			this.gameResultRepository = gameResultRepository;
		}

		public async Task<int> CreateGameResultAsync(CreateGameResultsModel model)
		{
			var dates = model.Week.Split(" - ");

			var fromDateString = dates[0];
			var toDateString = dates[1];

			DateTime fromDate;
			if (DateTime.TryParseExact(fromDateString, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDate) == false)
			{
				throw new ArgumentException(paramName: nameof(model.Week), message: "Invalid date format");
			}

			DateTime toDate;
			if (DateTime.TryParseExact(toDateString, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out toDate) == false)
			{
				throw new ArgumentException(paramName: nameof(model.Week), message: "Invalid date format");
			}

			var player = await this.playerRepository
				.GetAllAttached()
				.Where(p => p.IsDeleted == false)
				.Where(p => p.Id == model.PlayerId)
				.Include(p => p.Memberships)
				.FirstOrDefaultAsync();

			var membership = await this.membershipRepository
				.GetAllAttached()
				.Where(m => m.IsDeleted == false)
				.Where(m => m.ClubId == model.ClubId && m.PlayerId == model.PlayerId)
				.FirstOrDefaultAsync();


			if (membership == null)
			{
				membership = new Membership()
				{
					Player = player!,
					ClubId = model.ClubId
				};

				await this.membershipRepository.AddAsync(membership);
			}

			GameResult gameResult = new GameResult()
			{
				Membership = membership!,
				GameTypeId = model.GameTypeId,
				FromDate = fromDate,
				ToDate = toDate,
				Result = model.Result,
				Fee = model.Fee
			};

			await this.gameResultRepository.AddAsync(gameResult);

			return gameResult.Id;
		}

		public async Task<EditGameResultsModel?> GetGameResultForEditAsync(int id)
		{
			var gameResult = await gameResultRepository
				.GetAllAttached()
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

			return gameResult;
		}

		public async Task<GameResult?> EditGameResultAsync(EditGameResultsModel model, int id)
		{
			var dates = model.Week.Split(" - ");

			var fromDateString = dates[0];
			var toDateString = dates[1];

			DateTime fromDate;
			if (DateTime.TryParseExact(fromDateString, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDate) == false)
			{
				throw new ArgumentException(paramName: nameof(model.Week), message: "Invalid date format");
			}

			DateTime toDate;
			if (DateTime.TryParseExact(toDateString, FromDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out toDate) == false)
			{
				throw new ArgumentException(paramName: nameof(model.Week), message: "Invalid date format");
			}

			GameResult? gameResult = await gameResultRepository.GetByIdAsync(id);

			if (gameResult == null || gameResult.IsDeleted)
			{
				throw new ArgumentException("Result not found.");
			}

			var player = await membershipRepository.GetAllAttached()
				.Where(m => m.Id == gameResult.MembershipId)
				.AsNoTracking()
				.Select(m => m.Player)
				.FirstOrDefaultAsync();

			var membership = await membershipRepository.GetAllAttached()
				.Where(m => m.ClubId == model.ClubId && m.PlayerId == player!.Id)
				.AsNoTracking()
				.FirstOrDefaultAsync();

			if (membership == null)
			{
				membership = new Membership()
				{
					Player = player!,
					ClubId = model.ClubId
				};

				await membershipRepository.AddAsync(membership);
			}

			gameResult.Membership = membership;
			gameResult.GameTypeId = model.GameTypeId;
			gameResult.FromDate = fromDate;
			gameResult.ToDate = toDate;
			gameResult.Result = model.Result;
			gameResult.Fee = model.Fee;

			if (await gameResultRepository.UpdateAsync(gameResult))
			{
				return gameResult;
			}

			return null;
		}

		public async Task<bool> DeleteGameResultAsync(int id)
		{
			var gameResult = await gameResultRepository.GetByIdAsync(id);

			gameResult.IsDeleted = true;

			return await gameResultRepository.UpdateAsync(gameResult);
		}

		public async Task<DetailsGameResultModel?> GetGameResultsDetailsAsync(int id)
		{
			var gameResult = await gameResultRepository.GetAllAttached()
				.Where(pg => pg.Id == id)
				.Where(pg => pg.IsDeleted == false)
				.AsNoTracking()
				.Select(pg => new DetailsGameResultModel()
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

			return gameResult;
		}

		public async Task<IndexGameResultsModel> IndexGetAllGameResultsAsync(Week? week, int? clubId)
		{
			var gameResultsQuery = gameResultRepository.GetAllAttached()
				.Where(pg => pg.IsDeleted == false);

			if (week != null)
			{
				gameResultsQuery = gameResultsQuery
					.Where(gr => gr.FromDate.Equals(week.FirstDayOfWeek));
			}

			if (clubId != null)
			{
				gameResultsQuery = gameResultsQuery
					.Where(gr => gr.Membership.ClubId == clubId);
			}

			var gameResults = await gameResultsQuery
				.AsNoTracking()
				.Select(pg => new DetailsGameResultModel()
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
				.ToListAsync();

			var weeks = gameResultRepository.GetAllAttached()
				.AsNoTracking()
				.Select(gr => new Week(gr.FromDate))
				.Distinct();

			var clubs = await clubRepository.GetAllAttached()
				.Where(c => c.IsDeleted == false)
				.AsNoTracking()
				.ToListAsync();

			var model = new IndexGameResultsModel()
			{
				Weeks = weeks,
				Clubs = clubs,
				GameResults = gameResults
			};

			return model;
		}

		public async Task<IndexGameResultsModel> IndexGetAllGameResultsAsync(string user, Week? week, int? clubId)
		{
			var gameResultsQuery = gameResultRepository.GetAllAttached()
				.Where(pg => pg.IsDeleted == false);

			if (week != null)
			{
				gameResultsQuery = gameResultsQuery
					.Where(gr => gr.FromDate.Equals(week.FirstDayOfWeek));
			}

			if (clubId != null)
			{
				gameResultsQuery = gameResultsQuery
					.Where(gr => gr.Membership.ClubId == clubId);
			}

			var gameResults = await gameResultsQuery
				.Where(pg => pg.Membership.Player.User.UserName == user)
				.AsNoTracking()
				.Select(pg => new DetailsGameResultModel()
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
				.ToListAsync();

			var weeks = gameResultRepository.GetAllAttached()
				.AsNoTracking()
				.Select(gr => new Week(gr.FromDate))
				.Distinct();

			var clubs = await clubRepository.GetAllAttached()
				.Where(c => c.IsDeleted == false)
				.Where(c => c.Memberships.Any(m => m.Player.User.UserName == user))
				.AsNoTracking()
				.ToListAsync();

			foreach (var club in clubs)
			{
				Console.WriteLine(club.Name);
			}

			var model = new IndexGameResultsModel()
			{
				Weeks = weeks,
				Clubs = clubs,
				GameResults = gameResults
			};

			return model;
		}
	}
}
