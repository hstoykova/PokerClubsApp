using Itenso.TimePeriod;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MockQueryable;
using Moq;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Game;

namespace PokerClubsApp.Services.Tests
{
	[TestFixture]
	public class GameResultServiceTests
	{
		private IList<Player> playersData = new List<Player>()
		{
			new Player()
			{
				Id = 1,
				Nickname = "player1"
			},
			new Player()
			{
				Id = 2,
				Nickname = "player2"
			}
		};

		private IList<Membership> membersData = new List<Membership>()
		{
			new Membership()
			{
				Id = 1,
				PlayerId = 1,
				ClubId = 1
			},
			new Membership()
			{
				Id = 2,
				PlayerId = 2,
				ClubId = 2
			}
		};

		private IList<GameResult> gameResultsData = new List<GameResult>()
		{
			new GameResult()
			{
				Id = 1,
				MembershipId = 1,
				GameTypeId = 1,
				FromDate = DateTime.Now,
				ToDate = DateTime.Now.AddDays(7),
				Result = 5,
				Fee = 2,
				Membership = new Membership()
				{
					Player = new Player()
					{
						Nickname = "Vanko"
					},
					ClubId = 1
				}
			},
			new GameResult()
			{
				Id = 2,
				MembershipId = 2,
				GameTypeId = 2,
				FromDate = DateTime.Now,
				ToDate = DateTime.Now.AddDays(7),
				Result = 7,
				Fee = 3,
				Membership = new Membership()
				{
					Player = new Player()
					{
						Nickname = "Penko"
					},
					ClubId = 2
				}
			}
		};

		private Mock<IRepository<Club, int>> clubRepository;
		private Mock<IRepository<Player, int>> playerRepository;
		private Mock<IRepository<GameType, int>> gameTypeRepository;
		private Mock<IRepository<Membership, int>> membershipRepository;
		private Mock<IRepository<GameResult, int>> gameResultRepository;

		[SetUp]
		public void Setup()
		{
			this.clubRepository = new Mock<IRepository<Club, int>>();
			this.playerRepository = new Mock<IRepository<Player, int>>();
			this.gameTypeRepository = new Mock<IRepository<GameType, int>>();
			this.membershipRepository = new Mock<IRepository<Membership, int>>();
			this.gameResultRepository = new Mock<IRepository<GameResult, int>>();
		}

		[Test]
		public async Task CreateGameResultAsyncSuccess()
		{
			CreateGameResultsModel model = new CreateGameResultsModel()
			{
				PlayerId = 1,
				ClubId = 17487,
				Result = 15,
				Fee = 7,
				GameTypeId = 1,
				Week = "18-11-2024 - 24-11-2024"
			};

			IQueryable<Player> playersMockQueryable = playersData.BuildMock();

			playerRepository
				.Setup(r => r.GetAllAttached())
				.Returns(playersMockQueryable);

			IQueryable<Membership> membershipsMockQueriable = membersData.BuildMock();

			membershipRepository
				.Setup(r => r.GetAllAttached())
				.Returns(membershipsMockQueriable);

			membershipRepository
				.Setup(r => r.AddAsync(It.IsAny<Membership>()))
				.Returns(Task.CompletedTask);

			gameResultRepository
				.Setup(r => r.AddAsync(It.IsAny<GameResult>()))
				.Returns((GameResult gr) =>
				{
					gr.Id = 1;

					return Task.CompletedTask;
				});

			IGameResultService gameResultService = new GameResultService(clubRepository.Object, playerRepository.Object, gameTypeRepository.Object,
				membershipRepository.Object, gameResultRepository.Object);

			var gameResult = await gameResultService.CreateGameResultAsync(model);

			Assert.AreEqual(1, gameResult);
		}

		[Test]
		public async Task CreateGameResultAsyncFailure()
		{
			CreateGameResultsModel model = new CreateGameResultsModel()
			{
				PlayerId = 1,
				ClubId = 17487,
				Result = 15,
				Fee = 7,
				GameTypeId = 1,
				Week = "18-11-2024 - 24-11-2024"
			};

			IQueryable<Player> playersMockQueryable = playersData.BuildMock();

			playerRepository
				.Setup(r => r.GetAllAttached())
				.Returns(playersMockQueryable);

			IQueryable<Membership> membershipsMockQueriable = membersData.BuildMock();

			membershipRepository
				.Setup(r => r.GetAllAttached())
				.Returns(membershipsMockQueriable);

			membershipRepository
				.Setup(r => r.AddAsync(It.IsAny<Membership>()))
				.Returns(Task.CompletedTask);

			gameResultRepository
				.Setup(r => r.AddAsync(It.IsAny<GameResult>()))
				.ThrowsAsync(new Exception("failure"));

			IGameResultService gameResultService = new GameResultService(clubRepository.Object, playerRepository.Object, gameTypeRepository.Object,
				membershipRepository.Object, gameResultRepository.Object);

			var exception = Assert.ThrowsAsync<Exception>(() => gameResultService.CreateGameResultAsync(model));

			Assert.That(exception.Message, Is.EqualTo("failure"));
		}

		[Test]
		public async Task GetGameResultForEditAsyncSuccess()
		{
			IQueryable<GameResult> gameResultsMockQueriable = gameResultsData.BuildMock();

			gameResultRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gameResultsMockQueriable);

			IGameResultService gameResultService = new GameResultService(clubRepository.Object, playerRepository.Object, gameTypeRepository.Object,
				membershipRepository.Object, gameResultRepository.Object);

			var gameResult = await gameResultService.GetGameResultForEditAsync(1);

			Assert.That(gameResult, Is.Not.Null);
			Assert.AreEqual(5, gameResult.Result);
			Assert.AreEqual(2, gameResult.Fee);
		}

		[Test]
		public async Task GetGameResultForEditAsyncFailure1()
		{
			IQueryable<GameResult> gameResultsMockQueriable = new List<GameResult>().BuildMock();

			gameResultRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gameResultsMockQueriable);

			IGameResultService gameResultService = new GameResultService(clubRepository.Object, playerRepository.Object, gameTypeRepository.Object,
				membershipRepository.Object, gameResultRepository.Object);

			var gameResult = await gameResultService.GetGameResultForEditAsync(1);

			Assert.That(gameResult, Is.Null);
		}

		[Test]
		public async Task GetGameResultForEditAsyncFailure2()
		{
			IQueryable<GameResult> gameResultsMockQueriable = gameResultsData.BuildMock();

			gameResultRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gameResultsMockQueriable);

			IGameResultService gameResultService = new GameResultService(clubRepository.Object, playerRepository.Object, gameTypeRepository.Object,
				membershipRepository.Object, gameResultRepository.Object);

			var gameResult = await gameResultService.GetGameResultForEditAsync(3);

			Assert.That(gameResult, Is.Null);
		}
	}
}
