using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly VideoGameStoreContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly EmailService _emailService; // Assuming EmailService is the correct class name

        public EmailController(ILogger<ProductsController> logger, VideoGameStoreContext context, EmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("sendWelcomeEmail/{username}")]
        public IActionResult SendWelcomeEmail(string username)
        {
            // Get user details using username, e.g., from the database
            User user = _context.GetUserByUsername(username);

            // Send the welcome email using the instance of EmailService
            _emailService.SendWelcomeEmail(user).Wait();

            return Ok(new { message = "Welcome email sent successfully." });
        }
    }
}
