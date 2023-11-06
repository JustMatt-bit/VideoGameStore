using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly VideoGameStoreContext _context;

        public CheckoutController(ILogger<CheckoutController> logger, VideoGameStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        // api/cart/:id
        [HttpGet("{username}")]
        public ActionResult<IEnumerable<User>> Get(string username)
        {
            try
            {
                var userData = _context.GetUserByUsername(username);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
