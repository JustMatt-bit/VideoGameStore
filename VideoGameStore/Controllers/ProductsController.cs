using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly VideoGameStoreContext _context;

        public ProductsController(ILogger<ProductsController> logger, VideoGameStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        // api/products
        [HttpGet("get")]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                var products = _context.GetSellableProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetUserProducts/{username}")]
        public ActionResult<IEnumerable<Product>> GetUserProducts(string username)
        {
            try
            {
                var products = _context.GetUserProducts(username);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}