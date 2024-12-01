using Microsoft.AspNetCore.Identity;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Data
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> userManager;

        public AccountService(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task ResetPasswordAsync(SetPasswordModel model)
        {
            var existingUser = await userManager.FindByIdAsync(model.UserId);

            if (existingUser == null)
            {
                throw new ArgumentException("User not found");
            }

            var verified = await userManager.ResetPasswordAsync(existingUser, model.Token, model.Password);

            if (verified == null || !verified.Succeeded)
            {
                throw new ArgumentException("User not verified");
            }
        }
    }
}
