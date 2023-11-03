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
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                var products = _context.GetAllProducts();
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