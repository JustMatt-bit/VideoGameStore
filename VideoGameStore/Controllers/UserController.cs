using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using System;
using System.Reflection.Metadata;
using System.Xml.Linq;
using VideoGameStore.Controllers;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly VideoGameStoreContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly EmailService _emailService; // Assuming EmailService is the correct class name
        public UserController(ILogger<ProductsController> logger, VideoGameStoreContext context, EmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet("GetOrderHistory/{username}")]
        public ActionResult<List<Order>> GetOrderHistory(string username)
        {
            try
            {
                // Make a call to your VideoGameStoreContext or any service to get order history
                List<Order> orderHistory = _context.GetOrderHistoryByUsername(username);

                if (orderHistory != null)
                {
                    // Return order history if found
                    return Ok(orderHistory);
                }
                else
                {
                    // No orders found for the user
                    return NotFound(new { Message = "No order history found for the user" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                _logger.LogError(ex, "Error during fetching order history");
                return StatusCode(500, new { Message = "Internal server error" });
            }
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
            if (_context.GetVerificationStatus(model.username)){
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
            }else
            {
                return NotFound(new { Message = "Account not verified" });
            }
            return Unauthorized(new { Message = "Wrong password or username" });
        }

        public class LoginRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        [HttpPost("register")]
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

                _context.CreateNewBuildOrderFromUsername( registrationData.username );

                if (registrationSuccessful)
                {
                    // Generate a verification token
                    var verificationToken = Guid.NewGuid().ToString();

                    // Store the verification link in the database
                    _context.InsertVerificationLink(registrationData.username, verificationToken);

                    // Send the verification email
                    SendVerificationEmail(registrationData.email, registrationData.username, verificationToken);

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
        [HttpGet("GetUserDetails/{username}")]
        public ActionResult<User> GetUserDetails(string username)
        {
            try
            {
                // Make a call to your VideoGameStoreContext or any service to get user details
                User user = _context.GetUserByUsername(username);

                if (user != null)
                {
                    // Return user details if found
                    return Ok(user);
                }
                else
                {
                    // User not found
                    return NotFound(new { Message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        [HttpPut("edit")]
        public ActionResult Edit([FromBody] User updatedUserData)
        {
            try
            {
                // Get the current user from the database
                User existingUser = _context.GetUserByUsername(updatedUserData.username);

                if (existingUser == null)
                {
                    // User not found
                    return NotFound(new { Message = "no user found" });
                }

                // Update user information


                // Save changes to the database
                bool updateSuccessful = _context.UpdateUser(updatedUserData);

                if (updateSuccessful)
                {
                    return Ok(new { Message = updateSuccessful });
                }
                else
                {
                    return BadRequest(new { success = false, error = "Failed to update user information" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user information update");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetOrderById/{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            try
            {
                // Make a call to your VideoGameStoreContext or any service to get the order by ID
                Order order = _context.GetOrderByID(id);

                if (order != null)
                {
                    // Return the order if found
                    return Ok(order);
                }
                else
                {
                    // Order not found
                    return NotFound(new { Message = "Order not found" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }
        [HttpGet("GetCurrentUser")]
        public ActionResult<User> GetCurrentUser()
        {
            var username = HttpContext.Request.Cookies["AuthCookie"];
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { Message = "User not authenticated" });
            }

            try
            {
                User user = _context.GetUserByUsername(username);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }
        [HttpGet("VerifyAccount/{username}/{token}")]
        public ActionResult VerifyAccount(string username, string token)
        {
            try
            {
                // Retrieve the verification token stored in the database
                var storedToken = _context.GetVerificationLinkByUsername(username);
                _logger.LogError(storedToken);
                // Check if the provided token matches the stored token
                if (storedToken == token)
                {
                    // Update user account to mark it as verified
                    if(_context.VerifyUserAccount(username, storedToken))
                    {
                        _context.DeleteVerificationLinkByUsername(username);
                        return Ok(new { Message = "Account verified successfully" });
                    }
                  
                }
                return BadRequest(new { Message = "Invalid verification token" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during account verification");
                return StatusCode(500, "Internal server error");
            }
        }

        private void SendVerificationEmail(string userEmail, string username, string verificationToken)
        {
            // Construct the verification link manually
            var verificationLink = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/verify?username={username}&token={verificationToken}";

            // Rest of the method remains unchanged
            var subject = "Verify Your Account";
            var body = $"Click the following link to verify your account: {verificationLink}";

            _emailService.SendEmailAsync(userEmail, subject, body).Wait();
        }
        [HttpPost("DeactivateAccount")]
        public IActionResult DeactivateAccount()
        {
            var authCookie = HttpContext.Request.Cookies
       .FirstOrDefault(cookie => cookie.Key.StartsWith("AuthCookie_", StringComparison.OrdinalIgnoreCase));

            if (authCookie.Key != null)
            {
                var username = authCookie.Value;

                try
                {
                    // Implement logic to deactivate the account based on the authenticated user
                    bool accountDeactivationSuccessful = _context.DeactivateAccount(username);

                    if (accountDeactivationSuccessful)
                    {
                        // Optionally, you can also log the user out by removing the authentication cookie
                        // Delete all cookies by iterating through them
                        foreach (var cookie in HttpContext.Request.Cookies.Keys)
                        {
                            Response.Cookies.Delete(cookie);
                        }

                        return Ok(new { Message = "Account deactivated successfully" });
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed to deactivate account" });
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it accordingly
                    return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
                }
            }
            else
            {
                // Handle the case where the authentication cookie is not present
                return BadRequest(new { Message = "User not authenticated" });
            }
        }


    }
}
