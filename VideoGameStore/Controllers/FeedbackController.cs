using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger<FeedbackController> _logger;
        private readonly VideoGameStoreContext _context;

        public FeedbackController(ILogger<FeedbackController> logger, VideoGameStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        // api/feedback/#productId
        [HttpGet("{productId}")]
        public ActionResult<IEnumerable<Feedback>> Get(int productId)
        {
            try
            {
                var feedback = _context.GetFeedbackForProduct(productId);
                return Ok(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
