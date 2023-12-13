using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly VideoGameStoreContext _context;

    public LeaderboardController(VideoGameStoreContext context)
    {
        _context = context;
    }

    [HttpGet("GetTopUsers")]
    public ActionResult<IEnumerable<User>> GetTopUsers()
    {
        try
        {
            var topUsers = _context.GetTopUsersByLoyaltyProgress();
            return Ok(topUsers);
        }
        catch (Exception ex)
        {
            // Log error here
            return StatusCode(500, "Internal server error uwu");
        }
    }
}
