using Moq;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockQueryable;

namespace PokerClubsApp.Services.Tests
{
    [TestFixture]
    public class GameTypeServiceTests
    {
        private IList<GameType> gameTypeData = new List<GameType>()
        {
            new GameType()
            {
                Name = "MTT",
                Id = 1
            },
            new GameType()
            {
                Name = "SNG",
                Id = 2
            }
        };

        private Mock<IRepository<GameType, int>> gameTypeRepository;

        [SetUp]
        public void Setup()
        {
            this.gameTypeRepository = new Mock<IRepository<GameType, int>>();
        }

        [Test]
        public async Task GetAllGameTypesAsyncSuccess()
        {
            gameTypeRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(gameTypeData);

            IGameTypeService gameTypeService = new GameTypeService(gameTypeRepository.Object);
            var gameTypes = await gameTypeService.GetAllGameTypesAsync();

            Assert.That(gameTypes.Count(), Is.EqualTo(2));

            foreach (var gameType in gameTypes)
            {
                Assert.IsTrue(gameTypeData.Contains(gameType));
            }
        }

        [Test]
        public void GetAllGameTypesAsyncFailure()
        {
            gameTypeRepository
                .Setup(r => r.GetAllAsync())
                .ThrowsAsync(new Exception("failure"));

            IGameTypeService gameTypeService = new GameTypeService(gameTypeRepository.Object);
            var exception = Assert.ThrowsAsync<Exception>(() => gameTypeService.GetAllGameTypesAsync());

            Assert.That(exception.Message, Is.EqualTo("failure"));
        }
    }
}
