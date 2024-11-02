using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferralController : ControllerBase
    {
        private readonly IVideoGameStoreContext _context;

        public ReferralController(IVideoGameStoreContext context)
        {
            _context = context;
        }

        [HttpGet("CheckReferralCode/{referralCode}")]
        public IActionResult CheckReferralCode(string referralCode)
        {
            if (string.IsNullOrEmpty(referralCode))
            {
                return BadRequest("Referral code is required.");
            }

            bool referralCodeExists = _context.CheckReferralCodeExists(referralCode);

            if (referralCodeExists)
            {
                return Ok(new { Exists = true });
            }
            else
            {
                return NotFound(new { Exists = false });
            }
        }

        [HttpPost("GenerateReferralCode/{userName}")]
        public IActionResult GenerateReferralCode(string userName)
        {
            var username = userName;
            var referralCode = _context.GenerateReferralCode(username);
            return Ok(new { referralCode });
        }
    }
}
