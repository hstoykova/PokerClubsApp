using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Invitations;

namespace PokerClubsApp.Controllers
{
    public class InvitationsController : Controller
    {
        private readonly IClubService clubService;
        private readonly IInvitationService invitationService;

        public InvitationsController(IClubService clubService, IInvitationService invitationService)
        {
            this.clubService = clubService;
            this.invitationService = invitationService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Player()
        {
            var model = new PlayerInvitationModel();

            model.Clubs = (await clubService.GetAllClubsAsync()).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Player(PlayerInvitationModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Clubs = (await clubService.GetAllClubsAsync()).ToList();
                return View(model);
            }

            try
            {
                var invitation = await invitationService.CreatePlayerInvitationAsync(model);
                // TODO Redirect to new page
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                model.Clubs = (await clubService.GetAllClubsAsync()).ToList();

                return View(model);
            }
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            var model = new AdminInvitationModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin(AdminInvitationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var invitation = await invitationService.CreateAdminInvitationAsync(model);
                // TODO Redirect to new page
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);

                return View(model);
            }
        }
    }
}
