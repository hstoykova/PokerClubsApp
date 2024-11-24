using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using PokerClubsApp.Services.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Data
{
    public class UserSeedService : IUserSeedService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public UserSeedService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task SeedUserAsync()
        {
            string email = configuration["AdminAccount:Email"]!;
            string password = configuration["AdminAccount:Password"]!;

            var admin = await userManager.FindByEmailAsync(email);

            if (admin is not null)
            {
                return;
            }

            admin = new IdentityUser { Email = email, UserName = email };
            var result = await userManager.CreateAsync(admin, password);

            if (!result.Succeeded)
            {
                throw new ApplicationException("Failed to create admin user!");
            }

            var grantRole = await userManager.AddToRoleAsync(admin, "Admin");

            if (!grantRole.Succeeded)
            {
                throw new ApplicationException("Failed to add role to admin!");
            }
        }
    }
}
