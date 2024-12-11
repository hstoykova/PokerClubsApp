using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Moq;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Web.ViewModels.Invitations;
using Microsoft.AspNetCore.Identity.UI.Services;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Common;

namespace PokerClubsApp.Services.Tests
{
	[TestFixture]
	public class InvitationServiceTests
	{
		private Mock<IRepository<Player, int>> playerRepository;
		private Mock<IRepository<Membership, int>> membershipRepository;
		private Mock<UserManager<IdentityUser>> userManager;
		private Mock<IEmailSender> emailSender;
		private Mock<IHttpContextAccessor> httpContextAccessor;

		[SetUp]
		public void Setup()
		{
			this.playerRepository = new Mock<IRepository<Player, int>>();
			this.membershipRepository = new Mock<IRepository<Membership, int>>();
			var store = new Mock<IUserStore<IdentityUser>>();
			this.userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
			this.emailSender = new Mock<IEmailSender>();
			this.httpContextAccessor = new Mock<IHttpContextAccessor>();
		}

		[Test]
		public async Task CreatePlayerInvitationAsyncSuccess()
		{
			var model = new PlayerInvitationModel()
			{
				Email = "canko@abv.bg",
				Nickname = "canko123",
				AccountId = "12345",
				ClubId = 1
			};

			userManager
				.Setup(r => r.FindByEmailAsync(model.Email))
				.Returns(Task.FromResult<IdentityUser?>(null));

			userManager
				.Setup(r => r.CreateAsync(new IdentityUser()
				{
					Email = model.Email,
					UserName = model.Email
				}))
				.ReturnsAsync(IdentityResult.Success);

			userManager
				.Setup(r => r.AddToRoleAsync(It.IsAny<IdentityUser>(), "Player"))
				.ReturnsAsync(IdentityResult.Success);

			userManager
				.Setup(r => r.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()))
				.ReturnsAsync("abc123");

			playerRepository
				.Setup(r => r.AddAsync(It.IsAny<Player>()))
				.Returns(Task.CompletedTask);

			membershipRepository
				.Setup(r => r.AddAsync(It.IsAny<Membership>()))
				.Returns(Task.CompletedTask);

			emailSender
				.Setup(r => r.SendEmailAsync(model.Email, It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.CompletedTask);

			httpContextAccessor
				.Setup(r => r.HttpContext.Request.Host)
				.Returns(HostString.FromUriComponent("localhost"));

			IInvitationService invitationService = new InvitationService(playerRepository.Object, membershipRepository.Object, userManager.Object,
				emailSender.Object, httpContextAccessor.Object);

			Assert.DoesNotThrowAsync(() => invitationService.CreatePlayerInvitationAsync(model));
		}

		[Test]
		public async Task CreatePlayerInvitationAsyncFailure()
		{
			var model = new PlayerInvitationModel()
			{
				Email = "canko@abv.bg",
				Nickname = "canko123",
				AccountId = "12345",
				ClubId = 1
			};

			userManager
				.Setup(r => r.FindByEmailAsync(model.Email))
				.ReturnsAsync(new IdentityUser()
				{
					Email = model.Email
				});

			IInvitationService invitationService = new InvitationService(playerRepository.Object, membershipRepository.Object, userManager.Object,
				emailSender.Object, httpContextAccessor.Object);

			Assert.ThrowsAsync<ValidationException>(() => invitationService.CreatePlayerInvitationAsync(model), "User already invited");
		}

		[Test]
		public async Task CreateAdminInvitationAsyncSuccess()
		{
			var model = new AdminInvitationModel()
			{
				Email = "canko@abv.bg"
			};

			userManager
				.Setup(r => r.FindByEmailAsync(model.Email))
				.Returns(Task.FromResult<IdentityUser?>(null));

			userManager
				.Setup(r => r.CreateAsync(new IdentityUser()
				{
					Email = model.Email,
					UserName = model.Email
				}))
				.ReturnsAsync(IdentityResult.Success);

			userManager
				.Setup(r => r.AddToRoleAsync(It.IsAny<IdentityUser>(), "Admin"))
				.ReturnsAsync(IdentityResult.Success);

			userManager
				.Setup(r => r.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()))
				.ReturnsAsync("abc123");

			emailSender
				.Setup(r => r.SendEmailAsync(model.Email, It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.CompletedTask);

			httpContextAccessor
				.Setup(r => r.HttpContext.Request.Host)
				.Returns(HostString.FromUriComponent("localhost"));

			IInvitationService invitationService = new InvitationService(playerRepository.Object, membershipRepository.Object, userManager.Object,
				emailSender.Object, httpContextAccessor.Object);

			Assert.DoesNotThrowAsync(() => invitationService.CreateAdminInvitationAsync(model));
		}

		[Test]
		public async Task CreateAdminInvitationAsyncFailure()
		{
			var model = new AdminInvitationModel()
			{
				Email = "canko@abv.bg"
			};

			userManager
				.Setup(r => r.FindByEmailAsync(model.Email))
				.ReturnsAsync(new IdentityUser()
				{
					Email = model.Email
				});

			IInvitationService invitationService = new InvitationService(playerRepository.Object, membershipRepository.Object, userManager.Object,
				emailSender.Object, httpContextAccessor.Object);

			Assert.ThrowsAsync<ValidationException>(() => invitationService.CreateAdminInvitationAsync(model), "User already invited");
		}
	}
}
