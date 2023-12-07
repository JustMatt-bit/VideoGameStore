using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;


namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly VideoGameStoreContext _context;
        private readonly ILogger<ProductsController> _logger;

        public UserController(ILogger<ProductsController> logger, VideoGameStoreContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            // Validate model
            if (model == null || string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password))
            {
                return BadRequest("Invalid request");
            }

            // Attempt login
            bool isAuthenticated = _context.Login(HttpContext, model.username, model.password);

            if (isAuthenticated)
            {
                // Generate the token and set it in the response cookie
                var token = model.username; // You need to implement this method
                Response.Cookies.Append("AuthCookie_" + model.username, token, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false, // Set to true if using HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1), // Set an appropriate expiration time
                    Path = "/" // Set the cookie path
                });

                return Ok(new { Message = "Login successful", Token = token });
            }

            return Unauthorized(new { Message = "Wrong password" });
        }

        public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    [HttpPost()]
        public ActionResult Register([FromBody] User registrationData)
        {
            try
            {
                bool registrationSuccessful = _context.RegisterUser(
                    registrationData.username,
                    registrationData.password,
                    registrationData.name,
                    registrationData.surname,
                    registrationData.email,
                    registrationData.phone,
                    registrationData.referal_code
                );

                if (registrationSuccessful)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, error = "Registration failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
