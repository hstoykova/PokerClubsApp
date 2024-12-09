using Microsoft.AspNetCore.Identity;
using Moq;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Services.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Tests
{
    [TestFixture]
    public class RolesSeedServiceTests
    {
        private Mock<RoleManager<IdentityRole>> roleManager;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            this.roleManager = new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);
        }

        [Test]
        public void SeedRolesAsyncSuccess()
        {
            roleManager
                .Setup(r => r.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            roleManager
                .Setup(r => r.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            IRolesSeedService rolesService = new RolesSeedService(roleManager.Object);

            Assert.DoesNotThrowAsync(() => rolesService.SeedRolesAsync());
        }

        [Test]
        public void SeedRolesAsyncFailure()
        {
            roleManager
                .Setup(r => r.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            roleManager
                .Verify(r => r.CreateAsync(It.IsAny<IdentityRole>()), Times.Never());

            IRolesSeedService rolesService = new RolesSeedService(roleManager.Object);

            Assert.DoesNotThrowAsync(() => rolesService.SeedRolesAsync());
        }
    }
}
