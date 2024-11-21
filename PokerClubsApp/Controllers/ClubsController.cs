using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Clubs;
using PokerClubsApp.Web.ViewModels.Unions;

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
        public async Task<IActionResult> CreateAsync()
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
    }
}
