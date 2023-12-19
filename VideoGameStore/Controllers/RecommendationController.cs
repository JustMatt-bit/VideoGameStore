using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : Controller
    {
        private readonly ILogger<RecommendationController> _logger;
        private readonly VideoGameStoreContext _context;
        public RecommendationController(ILogger<RecommendationController> logger, VideoGameStoreContext context)
        {
            _logger = logger;
            _context = context;
        }



        [HttpGet("get/{username}")]
        public ActionResult<IEnumerable<Product>> Get(string username)
        {
            try
            {
                var products = _context.GetRecommendations(username);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recommendations");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
