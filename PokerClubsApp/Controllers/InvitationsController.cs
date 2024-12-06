using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokerClubsApp.Common;
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
                await invitationService.CreatePlayerInvitationAsync(model);
       
                return RedirectToAction("Index", "Home");
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Field, e.Message);
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
                await invitationService.CreateAdminInvitationAsync(model);
                
                return RedirectToAction("Index", "Home");
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Field, e.Message);

                return View(model);
            }
        }
    }
}
