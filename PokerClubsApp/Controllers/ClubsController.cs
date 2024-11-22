using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Clubs;
using PokerClubsApp.Web.ViewModels.GameResults;

namespace PokerClubsApp.Controllers
{
    public class ClubsController : Controller
    {
        private readonly PokerClubsDbContext context;

        public ClubsController(PokerClubsDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AddClubModel();
            model.Unions = await context.Unions.ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddClubModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Unions = await context.Unions.ToListAsync();
                return View(model);
            }

            Club club = new Club() 
            { 
                Name = model.Name,
                UnionId = model.UnionId
            };
            
            await context.Clubs.AddAsync(club);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Clubs
                .Where(c => c.IsDeleted == false)
                .Where(c => c.Id == id)
                .Select(c => new AddClubModel()
                {
                    Name = c.Name,
                    UnionId = c.UnionId
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            model.Unions = await context.Unions.ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddClubModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Unions = await context.Unions.ToListAsync();
                return View(model);
            }

            Club club = new Club()
            {
                Name = model.Name,
                UnionId = model.UnionId
            };

            await context.Clubs.AddAsync(club);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
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
    }
}
