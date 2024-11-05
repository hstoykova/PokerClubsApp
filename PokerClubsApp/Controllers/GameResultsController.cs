using Microsoft.AspNetCore.Mvc;

namespace PokerClubsApp.Controllers
{
    public class GameResultsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Add()
        {

            return this.View();
        }
    }
}
