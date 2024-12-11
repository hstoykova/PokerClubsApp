using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Services.Data;


namespace PokerClubsApp.Services.Tests
{
	[TestFixture]
	public class UserSeedServiceTests
	{
		private Mock<UserManager<IdentityUser>> userManager;
		private Mock<IConfiguration> configuration;

		[SetUp]
		public void Setup()
		{
			var store = new Mock<IUserStore<IdentityUser>>();
			this.userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
			this.configuration = new Mock<IConfiguration>();
		}

		[Test]
		public async Task SeedUserAsyncSuccess()
		{
			userManager
				.Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
				.Returns(Task.FromResult<IdentityUser?>(null));

			userManager
				.Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			userManager
				.Setup(r => r.AddToRoleAsync(It.IsAny<IdentityUser>(), "Admin"))
				.ReturnsAsync(IdentityResult.Success);

			IUserSeedService userSeedService = new UserSeedService(userManager.Object, configuration.Object);

			Assert.DoesNotThrowAsync(() => userSeedService.SeedUserAsync());
		}

		[Test]
		public async Task SeedUserAsyncFailure1()
		{
			userManager
				.Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
				.Returns(Task.FromResult<IdentityUser?>(null));

			userManager
				.Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Failed());

			IUserSeedService userSeedService = new UserSeedService(userManager.Object, configuration.Object);

			Assert.ThrowsAsync<ArgumentException>(() => userSeedService.SeedUserAsync(), "Failed to create admin user!");
		}

		[Test]
		public async Task SeedUserAsyncFailure2()
		{
			userManager
				.Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
				.Returns(Task.FromResult<IdentityUser?>(null));

			userManager
				.Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			userManager
				.Setup(r => r.AddToRoleAsync(It.IsAny<IdentityUser>(), "Admin"))
				.ReturnsAsync(IdentityResult.Failed());

			IUserSeedService userSeedService = new UserSeedService(userManager.Object, configuration.Object);

			Assert.ThrowsAsync<ArgumentException>(() => userSeedService.SeedUserAsync(), "Failed to add role to admin!");
		}
	}
}
