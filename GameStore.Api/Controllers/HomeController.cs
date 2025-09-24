using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    public class HomeController : Controller
    {
        // GET /
        public IActionResult Index()
        {
            return View();
        }
    }
}
