using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
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
                var cart_order_id = _context.GetOrdersByUser(username).Last().id;
                var cart_items = _context.GetCartItemsByUser(username);
                return Ok(new Object[] { cart_order_id, cart_items });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }
        // api/cart/:id
        [HttpPost()]
        public ActionResult Post([FromBody] CartChangeData data)
        {
            try
            {
                if (data == null) { return Ok(null); }
                _context.UpdateCartStock(data.oid, data.pid, data.val);
                return Ok(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }
        public class CartChangeData
        {
            public int oid { get; set; }
            public int pid { get; set; }
            public int val { get; set; }
        }
    }
}
