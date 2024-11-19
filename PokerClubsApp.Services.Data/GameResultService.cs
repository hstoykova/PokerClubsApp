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

        public async Task<int> AddGameResultAsync(AddGameResultsModel model)
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

            //if (fromDate.DayOfWeek != DayOfWeek.Monday || fromDate > toDate)
            //{
            //    throw new ArgumentException(paramName: nameof(model.FromDate), message: "From date must be Monday and earlier than To date!");
            //}

            //if (toDate.DayOfWeek != DayOfWeek.Sunday || toDate < fromDate)
            //{
            //    throw new ArgumentException(paramName: nameof(model.ToDate), message: "To date must be Sunday and later than From date!");
            //}

            var player = await this.playerRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Where(p => p.AccountId == model.AccountId)
                .Include(p => p.Memberships)
                .FirstOrDefaultAsync();


            // TODO If player is new - add button for adding new player

            //if (player is null)
            //{

            //}

            var membership = await this.membershipRepository
                .GetAllAttached()
                .Where(m => m.IsDeleted == false)
                .Where(m => m.ClubId == model.ClubId && m.PlayerAccountId == model.AccountId)
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
    }
}
