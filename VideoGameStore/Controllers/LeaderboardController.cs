using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
