using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Unions;

namespace PokerClubsApp.Controllers
{
    public class UnionsController : Controller
    {
        private readonly PokerClubsDbContext context;
        private readonly IUnionService unionService;

        public UnionsController(PokerClubsDbContext context, IUnionService unionService)
        {
            this.context = context;
            this.unionService = unionService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new CreateUnionModel();
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUnionModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var union = await unionService.CreateUnionAsync(model);

            return RedirectToAction(nameof(Details), new {id = union.Id});
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await unionService.GetUnionForEditAsync(id);

            if(model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CreateUnionModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var union = await unionService.EditUnionAsync(model, id);

            if (union == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id = union.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var model = await unionService.IndexGetAllUnionsAsync();
             
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var model = await context.Unions
                .Where(u => u.Id == id)
                .Where(u => u.IsDeleted == false)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if(model is null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
