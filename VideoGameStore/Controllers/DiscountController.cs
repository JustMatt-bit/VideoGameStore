using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;
using System.Collections.Generic;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly VideoGameStoreContext _context;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(VideoGameStoreContext context, ILogger<DiscountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/discount/userDiscounts/{username}
        [HttpGet("userDiscounts/{username}")]
        public ActionResult<IEnumerable<Discount>> GetUserDiscounts(string username)
        {
            try
            {
                var discounts = _context.GetDiscountsByUsername(username);
                if (discounts != null)
                {
                    return Ok(discounts);
                }
                return NotFound("No discounts found for user.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserDiscounts: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/discount/applyDiscount
        [HttpPost("applyDiscount")]
        public ActionResult ApplyDiscount([FromBody] DiscountApplication data)
        {
            try
            {
                var result = _context.ApplyDiscountToUser(data.Username, data.DiscountId);
                if (result)
                {
                    return Ok("Discount applied successfully.");
                }
                return BadRequest("Failed to apply discount.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ApplyDiscount: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("applyDiscountToOrder/{orderId}")]
        public ActionResult ApplyDiscountToOrder(int orderId, [FromBody] DiscountApplication data)
        {
            try
            {
                var result = _context.ApplyDiscountToOrder(orderId, data.DiscountId);
                if (result)
                {
                    return Ok("Discount applied to order successfully.");
                }
                return BadRequest("Failed to apply discount to order.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ApplyDiscountToOrder: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        public class DiscountApplication
        {
            public string? Username { get; set; }
            public int DiscountId { get; set; }
        }
    }
}

