using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Moq;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private Mock<UserManager<IdentityUser>> userManager;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            this.userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Test]
        public void ResetPasswordAsyncSuccess()
        {
            IdentityUser user = new IdentityUser();

            userManager
                .Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            userManager
                .Setup(r => r.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            IAccountService accountService = new AccountService(userManager.Object);

            var model = new SetPasswordModel()
            {
                Password = "123Aaa",
                ConfirmPassword = "123Aaa",
                UserId = "1",
                Token = "p9wr23"
            };

            Assert.DoesNotThrowAsync(() => accountService.ResetPasswordAsync(model));
        }

        [Test]
        public void ResetPasswordAsyncFailure1()
        {
            IdentityUser user = new IdentityUser();

            userManager
                .Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityUser?>(null));

            IAccountService accountService = new AccountService(userManager.Object);

            var model = new SetPasswordModel()
            {
                Password = "123Aaa",
                ConfirmPassword = "123Aaa",
                UserId = "1",
                Token = "p9wr23"
            };

            Assert.ThrowsAsync<ArgumentException>(() => accountService.ResetPasswordAsync(model), "User not found");
        }

        [Test]
        public void ResetPasswordAsyncFailure2()
        {
            IdentityUser user = new IdentityUser();

            userManager
                .Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            userManager
                .Setup(r => r.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityResult?>(null));

            IAccountService accountService = new AccountService(userManager.Object);

            var model = new SetPasswordModel()
            {
                Password = "123Aaa",
                ConfirmPassword = "123Aaa",
                UserId = "1",
                Token = "p9wr23"
            };

            Assert.ThrowsAsync<ArgumentException>(() => accountService.ResetPasswordAsync(model), "User not verified");
        }
    }
}
