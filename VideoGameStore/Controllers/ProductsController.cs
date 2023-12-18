using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using VideoGameStore.Models;
using System.IO;
using System.Threading.Channels;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly VideoGameStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ILogger<ProductsController> logger, VideoGameStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpGet("GetProductGenres/{id}")]
        public ActionResult<IEnumerable<int>> GetProductGenres(int id)
        {
            try
            {
                var genres = _context.GetProductGenres(id);
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product genres");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("GetGenres")]
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

        [HttpPost("GetGameTypes")]
        public ActionResult<IEnumerable<GameType>> GetGameTypes()
        {
            try
            {
                var gameTypes = _context.GetGameTypes();
                return Ok(gameTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("GetDevelopers")]
        public ActionResult<IEnumerable<Developer>> GetDevelopers()
        {
            try
            {
                var developers = _context.GetDevelopers();
                return Ok(developers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateDeveloper")]
        public ActionResult<int> CreateDeveloper([FromBody] Developer developer)
        {
            try
            {  
                var id = _context.CreateDeveloper(developer.name, developer.country);
                return Ok(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating developer");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("DeleteDeveloper")]
        public ActionResult<bool> DeleteDeveloper([FromBody] int id)
        {
            try
            {
                var deleted = _context.DeleteDeveloper(id);
                return Ok(deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating developer");
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
        [HttpPost("DeleteGenres")]
        public ActionResult<bool> DeleteGenres([FromBody] List<Genre> genres)
        {
            try
            {
                var deleteGenres = _context.DeleteGenres(genres);
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting genres");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("UpdateGenresProductConnection")]
        public ActionResult<bool> UpdateGenresProductConnection([FromBody] GenreProductRelation genreProductRelation)
        {
            try
            {
                var deleted = _context.DeleteGenresProductConnection(genreProductRelation.id);
                var completed = _context.GenresProductConnection(genreProductRelation.id, genreProductRelation.genres);
                return Ok(completed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product to genre connection");
                return StatusCode(500, "Internal server error");
            }
        }
        public class FileInfo
        {
            public int id { get; set; }
            public IFormFile image { get; set; }
        }



        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] FileInfo file)
        {
            try
            {
                if (file.image.Length > 0)
                {
                    var stringarr = file.image.FileName.Split('.');
                    var extension = stringarr[stringarr.Length - 1];
                    var newFileName = file.id.ToString() + "." + extension;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "../ClientApp/public/images/", newFileName);
                    Console.WriteLine(path);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await file.image.CopyToAsync(stream);
                        stream.Close();
                    }
                    var changed = _context.ChangeProductImage(file.id, newFileName);
                    return Ok(changed);
                }
                return NotFound(new { Message = "Image not found" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateProduct")]
        public ActionResult<int> CreateProduct([FromBody] Product product)
        {
            try
            {
                var id = _context.CreateProduct(product);
                return Ok(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("UpdateProduct")]
        public ActionResult<bool> UpdateProduct([FromBody] Product product)
        {
            try
            {
                var success = _context.UpdateProduct(product);
                return Ok(success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "Internal server error");
            }
        }

        public class GenreProductRelation
        {
            public int id { get; set; }
            public List<Genre> genres { get; set; }
        }

        [HttpPost("GenresProductConnection")]
        public ActionResult<bool> GenresProductConnection([FromBody] GenreProductRelation genreProductRelation)
        {
            try
            {
                var completed = _context.GenresProductConnection(genreProductRelation.id, genreProductRelation.genres);
                return Ok(completed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product connection to genres");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}