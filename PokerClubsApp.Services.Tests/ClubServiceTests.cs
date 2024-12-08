using Moq;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Web.ViewModels.Unions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerClubsApp.Web.ViewModels.Clubs;
using MockQueryable;

namespace PokerClubsApp.Services.Tests
{
    [TestFixture]
    public class ClubServiceTests
    {
        private IList<Club> clubsData = new List<Club>()
        {
            new Club()
            {
                Name = "First",
                Id = 1
            },
            new Club()
            {
                Name = "Second",
                Id = 2
            }
        };

        private Mock<IRepository<Club, int>> clubRepository;

        [SetUp]
        public void Setup()
        {
            this.clubRepository = new Mock<IRepository<Club, int>>();
        }

        [Test]
        public async Task CreateClubAsyncSuccess()
        {
            Club club = new Club()
            {
                Name = "Kiki"
            };

            clubRepository
                .Setup(r => r.AddAsync(club))
                .Returns(Task.CompletedTask);

            IClubService clubService = new ClubService(clubRepository.Object);

            var model = new CreateClubModel() { Name = "Kiki" };
            var createdClub = await clubService.CreateClubAsync(model);

            Assert.That(createdClub.Name, Is.EqualTo(club.Name));
        }

        [Test]
        public void CreateClubAsyncFailure()
        {
            clubRepository
                .Setup(r => r.AddAsync(It.IsAny<Club>()))
                .ThrowsAsync(new Exception("failure"));

            IClubService clubService = new ClubService(clubRepository.Object);

            var model = new CreateClubModel() { Name = "Kiki" };
            var exception = Assert.ThrowsAsync<Exception>(() => clubService.CreateClubAsync(model));

            Assert.That(exception.Message, Is.EqualTo("failure"));
        }

        [Test]
        public async Task GetClubForEditAsyncSuccess()
        {
            IQueryable<Club> clubsMockQueryable = clubsData.BuildMock();

            clubRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(clubsMockQueryable.First());

            IClubService clubService = new ClubService(clubRepository.Object);
            var model = await clubService.GetClubForEditAsync(1);

            Assert.That(model.Name, Is.EqualTo("First"));
        }

        [Test]
        public async Task GetClubForEditAsyncFailure()
        {
            clubRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Club?>(null));

            IClubService clubService = new ClubService(clubRepository.Object);
            var model = await clubService.GetClubForEditAsync(3);

            Assert.That(model, Is.Null);
        }

        [Test]
        public async Task EditClubAsyncSuccess()
        {
            clubRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Club() { Name = "Third", Id = 3 });

            clubRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Club>()))
                .ReturnsAsync(true);

            IClubService clubService = new ClubService(clubRepository.Object);
            var club = await clubService.EditClubAsync(new CreateClubModel() { Name = "23weeks" }, 3);

            Assert.That(club, Is.Not.Null);
            Assert.That(club.Name, Is.EqualTo("23weeks"));
        }

        [Test]
        public async Task EditClubAsyncFailure()
        {
            clubRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Club() { Name = "Third", Id = 3, IsDeleted = true });

            clubRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Club>()))
                .ReturnsAsync(false);

            IClubService clubService = new ClubService(clubRepository.Object);
            var club = await clubService.EditClubAsync(new CreateClubModel() { Name = "23weeks" }, 3);

            Assert.That(club, Is.Null);
        }

        [Test]
        public async Task GetAllClubsAsync()
        {
            IQueryable<Club> clubsMockQueryable = clubsData.BuildMock();

            clubRepository
                .Setup(r => r.GetAllAttached())
                .Returns(clubsMockQueryable);

            IClubService clubService = new ClubService(clubRepository.Object);
            var model = await clubService.GetAllClubsAsync();

            Assert.That(model.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task IndexGetAllClubsAsync()
        {
            IQueryable<Club> clubsMockQueryable = clubsData.BuildMock();

            clubRepository
                .Setup(r => r.GetAllAttached())
                .Returns(clubsMockQueryable);

            IClubService clubService = new ClubService(clubRepository.Object);
            var model = await clubService.IndexGetAllClubsAsync();

            Assert.That(model.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetClubDetailsAsyncSuccess()
        {
            IQueryable<Club> clubsMockQueryable = clubsData.BuildMock();

            clubRepository
                .Setup(r => r.GetAllAttached())
                .Returns(clubsMockQueryable);

            IClubService clubService = new ClubService(clubRepository.Object);
            var model = await clubService.GetClubDetailsAsync(2);

            Assert.That(model, Is.Not.Null);
            Assert.That(model.Id, Is.EqualTo(2));
        }

        [Test]
        public async Task GetClubDetailsAsyncFailure()
        {
            IQueryable<Club> clubsMockQueryable = clubsData.BuildMock();

            clubRepository
                .Setup(r => r.GetAllAttached())
                .Returns(clubsMockQueryable);

            IClubService clubService = new ClubService(clubRepository.Object);
            var model = await clubService.GetClubDetailsAsync(3);

            Assert.That(model, Is.Null);
        }
    }
}
