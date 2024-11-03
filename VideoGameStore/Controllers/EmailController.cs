using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IVideoGameStoreContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IEmailService _emailService;

        public EmailController(ILogger<ProductsController> logger, IVideoGameStoreContext context, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("sendWelcomeEmail/{username}")]
        public async Task<IActionResult> SendWelcomeEmail(string username)
        {
            User user = _context.GetUserByUsername(username);

            if (user == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            var result = await _emailService.SendWelcomeEmail(user);

            if (result.Success)
            {
                return Ok(new { message = "Welcome email sent successfully." });
            }
            else
            {
                return BadRequest(new { message = result.Message });
            }
        }


    }
}
