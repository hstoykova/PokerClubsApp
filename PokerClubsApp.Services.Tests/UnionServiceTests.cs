using MockQueryable;
using Moq;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Unions;

namespace PokerClubsApp.Services.Tests
{
    [TestFixture]
    public class UnionServiceTests
    {
        private IList<Union> unionsData = new List<Union>()
        {
            new Union()
            {
                Name = "First",
                Id = 1
            },
            new Union()
            {
                Name = "Second",
                Id = 2
            }
        };

        private Mock<IRepository<Union, int>> unionRepository;

        [SetUp]
        public void Setup()
        {
            this.unionRepository = new Mock<IRepository<Union, int>>();
        }

        [Test]
        public async Task CreateUnionAsyncSuccess()
        {
            Union union = new Union()
            {
                Name = "Meow"
            };

            unionRepository
                .Setup(r => r.AddAsync(union))
                .Returns(Task.CompletedTask);

            IUnionService unionService = new UnionService(unionRepository.Object);

            var model = new CreateUnionModel() { Name = "Meow" };
            var createdUnion = await unionService.CreateUnionAsync(model);

            Assert.That(createdUnion.Name, Is.EqualTo(union.Name));
        }

        [Test]
        public void CreateUnionAsyncFailure()
        {
            unionRepository
                .Setup(r => r.AddAsync(It.IsAny<Union>()))
                .ThrowsAsync(new Exception("failure"));

            IUnionService unionService = new UnionService(unionRepository.Object);

            var model = new CreateUnionModel() { Name = "Meow" };
            var exception = Assert.ThrowsAsync<Exception>(() => unionService.CreateUnionAsync(model));

            Assert.That(exception.Message, Is.EqualTo("failure"));
        }

        [Test]
        public async Task GetUnionForEditAsyncSuccess()
        {
            IQueryable<Union> unionsMockQueryable = unionsData.BuildMock();

            unionRepository
                .Setup(r => r.GetAllAttached())
                .Returns(unionsMockQueryable);

            IUnionService unionService = new UnionService(unionRepository.Object);
            var model = await unionService.GetUnionForEditAsync(1);

            Assert.That(model.Name, Is.EqualTo("First"));
        }

        [Test]
        public async Task GetUnionForEditAsyncFailure()
        {
            IQueryable<Union> unionsMockQueryable = unionsData.BuildMock();

            unionRepository
                .Setup(r => r.GetAllAttached())
                .Returns(unionsMockQueryable);

            IUnionService unionService = new UnionService(unionRepository.Object);
            var model = await unionService.GetUnionForEditAsync(3);

            Assert.That(model, Is.Null);
        }

        [Test]
        public async Task EditUnionAsyncSuccess()
        {
            unionRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Union() { Name = "Third", Id = 3 });

            unionRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Union>()))
                .ReturnsAsync(true);

            IUnionService unionService = new UnionService(unionRepository.Object);
            var union = await unionService.EditUnionAsync(new CreateUnionModel() { Name = "23weeks"}, 3);

            Assert.That(union, Is.Not.Null);
            Assert.That(union.Name, Is.EqualTo("23weeks"));
        }

        [Test]
        public async Task EditUnionAsyncFailure()
        {
            unionRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Union() { Name = "Third", Id = 3, IsDeleted = true });

            unionRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Union>()))
                .ReturnsAsync(false);

            IUnionService unionService = new UnionService(unionRepository.Object);
            var union = await unionService.EditUnionAsync(new CreateUnionModel() { Name = "23weeks" }, 3);

            Assert.That(union, Is.Null);
        }

        [Test]
        public async Task IndexGetAllUnionsAsync()
        {
            IQueryable<Union> unionsMockQueryable = unionsData.BuildMock();

            unionRepository
                .Setup(r => r.GetAllAttached())
                .Returns(unionsMockQueryable);

            IUnionService unionService = new UnionService(unionRepository.Object);
            var model = await unionService.IndexGetAllUnionsAsync();

            Assert.That(model.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUnionDetailsAsyncSuccess()
        {
            IQueryable<Union> unionsMockQueryable = unionsData.BuildMock();

            unionRepository
                .Setup(r => r.GetAllAttached())
                .Returns(unionsMockQueryable);

            IUnionService unionService = new UnionService(unionRepository.Object);
            var model = await unionService.GetUnionDetailsAsync(2);

            Assert.That(model, Is.Not.Null);
            Assert.That(model.Id, Is.EqualTo(2));
        }

        [Test]
        public async Task GetUnionDetailsAsyncFailure()
        {
            IQueryable<Union> unionsMockQueryable = unionsData.BuildMock();

            unionRepository
                .Setup(r => r.GetAllAttached())
                .Returns(unionsMockQueryable);

            IUnionService unionService = new UnionService(unionRepository.Object);
            var model = await unionService.GetUnionDetailsAsync(3);

            Assert.That(model, Is.Null);
        }
    }
}