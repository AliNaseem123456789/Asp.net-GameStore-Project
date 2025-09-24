using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    public class AuthController : Controller
    {
        // GET /
        public IActionResult Index()
        {
            return View();
        }
    }
}
