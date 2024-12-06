using Microsoft.AspNetCore.Mvc;

namespace PokerClubsApp.Controllers
{
	public class ErrorsController : Controller
	{
		[Route("Errors/{statusCode}")]
		public IActionResult Details(int statusCode)
		{
			string? viewName = null;

			if (statusCode == 404)
			{
				viewName = "NotFound";
			}
			else if (statusCode == 500)
			{
				viewName = "ServerError";
			}
			else
			{
				viewName = "OtherError";
			}

			return View(viewName, statusCode);
		}
	}
}
