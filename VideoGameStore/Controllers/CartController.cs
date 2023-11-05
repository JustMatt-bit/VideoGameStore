using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly VideoGameStoreContext _context;

        public CartController(ILogger<CartController> logger, VideoGameStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        // api/cart/:id
        [HttpGet("{username}")]
        public ActionResult<IEnumerable<Product>> Get(string username)
        {
            try
            {
                var cart_items = _context.GetCartItemsByUser(username);
                return Ok(cart_items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
