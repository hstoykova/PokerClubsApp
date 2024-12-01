using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Accounts;
using System.Security.Principal;

namespace PokerClubsApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService accountService;

        public AccountsController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FinishRegistration(string userId, string token)
        {
            var model = new SetPasswordModel()
            {
                UserId = userId,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> FinishRegistration(SetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await accountService.ResetPasswordAsync(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
