using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoGameStore.Controllers;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.IntegrationTests
{
    public class EmailControllerTests
    {
        private readonly Mock<IVideoGameStoreContext> _mockContext;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly Mock<IEmailService> _mockEmailService;

        public EmailControllerTests()
        {
            _mockContext = new Mock<IVideoGameStoreContext>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _mockEmailService = new Mock<IEmailService>(); // Use IEmailService here
        }

        [Fact]
        public async Task SendWelcomeEmail_ReturnsOkResult_WhenEmailSentSuccessfully()
        {
            // Arrange
            var user = new User { email = "testuser@example.com", username = "testuser", name = "Test User" };
    
            _mockContext.Setup(c => c.GetUserByUsername("testuser")).Returns(user);
            _mockEmailService
                .Setup(s => s.SendWelcomeEmail(It.IsAny<User>()))
                .ReturnsAsync(new ResultType { Success = true, Message = "Welcome email sent successfully." });

            var controller = new EmailController(_mockLogger.Object, _mockContext.Object, _mockEmailService.Object);

            // Act
            var result = await controller.SendWelcomeEmail("testuser");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value as dynamic;  // Casting as dynamic

            Assert.Equal("Welcome email sent successfully.", returnValue.message);
            _mockEmailService.Verify(s => s.SendWelcomeEmail(It.IsAny<User>()), Times.Once);
        }


        [Fact]
        public async Task SendWelcomeEmail_ReturnsErrorResult_WhenEmailFailsToSend()
        {
            // Arrange
            var user = new User { email = "invaliduser@example.com", username = "invaliduser", name = "Invalid User" };

            _mockContext.Setup(c => c.GetUserByUsername("invaliduser")).Returns(user);
            _mockEmailService
                .Setup(s => s.SendWelcomeEmail(It.IsAny<User>()))
                .ReturnsAsync(new ResultType { Success = false, Message = "Error sending welcome email" });

            var controller = new EmailController(_mockLogger.Object, _mockContext.Object, _mockEmailService.Object);

            // Act
            var result = await controller.SendWelcomeEmail("invaliduser");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // Cast the result as dynamic and check the message
            var returnValue = badRequestResult.Value as dynamic;
            Assert.Equal("Error sending welcome email", returnValue.message);

            _mockEmailService.Verify(s => s.SendWelcomeEmail(It.IsAny<User>()), Times.Once);
        }
    }
}
