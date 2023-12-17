using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

[ApiController]
[Route("api/[controller]")]
public class LoyaltyController : ControllerBase
{
    private readonly VideoGameStoreContext _context;

    public LoyaltyController(VideoGameStoreContext context)
    {
        _context = context;
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

}
