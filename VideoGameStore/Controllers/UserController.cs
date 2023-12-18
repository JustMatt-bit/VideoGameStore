using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata;
using System.Xml.Linq;
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

        [HttpGet("GetOrderHistory/{username}")]
        public ActionResult<List<Order>> GetOrderHistory(string username)
        {
            try
            {
                // Make a call to your VideoGameStoreContext or any service to get order history
                List<Order>orderHistory = _context.GetOrderHistoryByUsername(username);

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
