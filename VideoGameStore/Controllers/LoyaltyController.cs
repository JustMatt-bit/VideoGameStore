using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

[ApiController]
[Route("api/[controller]")]
public class LoyaltyController : ControllerBase
{
    private readonly IVideoGameStoreContext _context;
    private readonly ILogger<LoyaltyController> _logger;

    public LoyaltyController(IVideoGameStoreContext context, ILogger<LoyaltyController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("GetUserTierDetails/{username}")]
    public ActionResult GetUserTierDetails(string username)
    {
        var user = _context.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var loyaltyTier = _context.GetUserLoyaltyTier(username);

        if (loyaltyTier == null)
        {
            return NotFound("Loyalty tier not found for the user.");
        }

        var nextTier = _context.GetNextLoyaltyTier(loyaltyTier.TierId);

        // Calculate the points remaining to reach the next tier
        var pointsToNextTier = nextTier != null ? loyaltyTier.PointsTo - user.loyalty_progress : 0;

        return Ok(new
        {
            CurrentTier = loyaltyTier,
            NextTier = nextTier,
            PointsToNextTier = pointsToNextTier,
            UserProgress = user.loyalty_progress
        });
    }

    [HttpPost("updateUserLoyaltyPoints")]
    public IActionResult UpdateUserLoyaltyPoints([FromBody] LoyaltyPointsUpdateModel model)
    {
        if (model == null)
        {
            _logger.LogWarning("UpdateUserLoyaltyPoints received a null model");
            return BadRequest("Invalid request.");
        }

        _logger.LogInformation($"Updating loyalty points for user {model.Username}");
        _context.UpdateUserLoyaltyProgress(model.Username, model.LoyaltyPoints);

        _logger.LogInformation("User loyalty points updated successfully.");
        return Ok("User loyalty points updated.");
    }

    public class LoyaltyPointsUpdateModel
    {
        public string Username { get; set; }
        public double LoyaltyPoints { get; set; }
    }

}
