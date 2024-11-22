using Microsoft.AspNetCore.Mvc;
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

            return RedirectToAction("Index", "Home");
        }
    }
}
