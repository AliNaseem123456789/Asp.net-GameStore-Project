using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    public class GamesController : Controller
    {
        // GET /Games
        public IActionResult Index()
        {
            return View();
        }
    }
}
