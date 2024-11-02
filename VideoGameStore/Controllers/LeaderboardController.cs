using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly IVideoGameStoreContext _context;

    public LeaderboardController(IVideoGameStoreContext context)
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

    [HttpGet("GetUserPosition/{username}")]
    public ActionResult<int> GetUserPosition(string username)
    {
        try
        {
            int position = _context.GetUserPositionByUsername(username);
            if (position != -1)
            {
                return Ok(position);
            }
            return NotFound("User not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error uwu");
        }
    }
}
