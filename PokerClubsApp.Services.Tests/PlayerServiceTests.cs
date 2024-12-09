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
    public class PlayerServiceTests
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

        private Mock<IRepository<Player, int>> playerRepository;

        [SetUp]
        public void Setup()
        {
            this.playerRepository = new Mock<IRepository<Player, int>>();
        }

        [Test]
        public async Task GetAllPlayersAsync()
        {
            IQueryable<Player> playersMockQueryable = playersData.BuildMock();

            playerRepository
                .Setup(r => r.GetAllAttached())
                .Returns(playersMockQueryable);

            IPlayerService playerService = new PlayerService(playerRepository.Object);
            var players = await playerService.GetAllPlayersAsync();

            Assert.That(players.Count(), Is.EqualTo(2));

            foreach (var player in players)
            {
                Assert.IsTrue(playersData.Contains(player));
            }
        }
    }
}
