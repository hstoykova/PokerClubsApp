using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Unions;

namespace PokerClubsApp.Controllers
{
    public class UnionsController : Controller
    {
        private readonly PokerClubsDbContext context;

        public UnionsController(PokerClubsDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AddUnionModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddUnionModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Union union = new Union() { Name = model.Name };
            await context.Unions.AddAsync(union);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new {id = union.Id});
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Unions
                .Where(u => u.Id == id)
                .Where(u => u.IsDeleted == false)
                .AsNoTracking()
                .Select(u => new AddUnionModel()
                {
                    Name = u.Name
                })
                .FirstOrDefaultAsync();

            if(model is null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(AddUnionModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var union = await context.Unions
                .Where(u => u.Id == id)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync();

            union!.Name = model.Name;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = union.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await context.Unions
                .Where(u => u.IsDeleted == false)
                .AsNoTracking()
                .ToListAsync();
             
            return View(model);
        }

        [HttpGet]
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
