using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
