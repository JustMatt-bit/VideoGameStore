using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger<FeedbackController> _logger;
        private readonly IVideoGameStoreContext _context;

        public FeedbackController(ILogger<FeedbackController> logger, IVideoGameStoreContext context)
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

        public class FeedbackRequestModel
        {
            public Feedback Feedback { get; set; }
            public string Username { get; set; }
        }

        [HttpPost("{productId}")]
        public IActionResult CreateFeedback(int productId, [FromBody] FeedbackRequestModel request)
        {
            try
            {
                bool isSuccess = _context.CreateFeedbackForProduct(productId, request.Feedback, request.Username);
                if (isSuccess)
                {
                    return Ok(); // or return CreatedAtRoute if you want to return the created entity
                }

                return BadRequest("Failed to save feedback");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating feedback");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("report/{feedbackId}")]
        public IActionResult ReportFeedback(int feedbackId)
        {
            try
            {
                bool isSuccess = _context.ReportFeedback(feedbackId);
                if (isSuccess)
                {
                    return Ok();
                }
                return BadRequest("Failed to report feedback");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reporting feedback");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("rate/{feedbackId}")]
        public IActionResult RateFeedback(int feedbackId, [FromBody] int newRating)
        {
            try
            {
                bool isSuccess = _context.RateFeedback(feedbackId, newRating);
                if (isSuccess)
                {
                    return Ok();
                }
                return BadRequest("Failed to update rating");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feedback rating");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
