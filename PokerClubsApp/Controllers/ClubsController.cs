using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Clubs;
using PokerClubsApp.Web.ViewModels.GameResults;

namespace PokerClubsApp.Controllers
{
    public class ClubsController : Controller
    {
        private readonly PokerClubsDbContext context;
        private readonly IClubService clubService;

        public ClubsController(PokerClubsDbContext context, IClubService clubService)
        {
            this.context = context;
            this.clubService = clubService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateClubModel();
            model.Unions = await context.Unions.ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Unions = await context.Unions.ToListAsync();
                return View(model);
            }

            var club = await clubService.CreateClubAsync(model);

            return RedirectToAction(nameof(Details), new {id = club.Id});
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await clubService.GetClubForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            // TODO: Take Unions from UnionsService !!!
            model.Unions = await context.Unions.ToListAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CreateClubModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                model.Unions = await context.Unions.ToListAsync();
                return View(model);
            }

            var club = await clubService.EditClubAsync(model, id);

            if (club == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new {id = club.Id});
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await context.Clubs
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Union)
                .AsNoTracking()           
                .ToListAsync();
                
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await context.Clubs
                .Where(c => c.Id == id)
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Union)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
