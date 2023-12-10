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

        [HttpGet("GetProduct/{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            try
            {
                var product = _context.GetProduct(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetGenres")]
        public ActionResult<IEnumerable<Genre>> GetGenres()
        {
            try
            {
                var genres = _context.GetGenres();
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("GenreExists")]
        public ActionResult<bool> GenreExists([FromBody] string name)
        {
            try
            {
                var genres = _context.GenreExists(name);
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if genre exists");
                return StatusCode(500, "Internal server error");
            }
        }

        public class CreatedGenre
        {
            public string name { get; set; }
            public string description { get; set; }

        }

        [HttpPost("CreateGenre")]
        public ActionResult<bool> CreateGenre([FromBody] CreatedGenre genre)
        {
            try
            {
                var genres = _context.CreateGenre(genre.name, genre.description);
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if genre exists");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}