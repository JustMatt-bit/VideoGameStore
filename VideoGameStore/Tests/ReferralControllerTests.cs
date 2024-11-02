using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VideoGameStore.Controllers;
using VideoGameStore.Models;
using static VideoGameStore.Controllers.ReferralController;

namespace VideoGameStore.Tests
{
    public class ReferralControllerTests
    {
        private readonly Mock<IVideoGameStoreContext> _contextMock;
        private readonly ReferralController _controller;

        public ReferralControllerTests()
        {
            _contextMock = new Mock<IVideoGameStoreContext>();
            _controller = new ReferralController(_contextMock.Object);
        }

        [Theory]
        [InlineData("VALIDCODE", true, 200)]   // Existing referral code, should return 200 OK
        [InlineData("INVALIDCODE", false, 404)] // Non-existent referral code, should return 404 Not Found
        [InlineData("", false, 400)]           // Empty referral code, should return 400 Bad Request
        public void CheckReferralCode_ReturnsExpectedResult(string referralCode, bool exists, int expectedStatusCode)
        {
            // Arrange
            if (!string.IsNullOrEmpty(referralCode))
            {
                _contextMock.Setup(c => c.CheckReferralCodeExists(referralCode)).Returns(exists);
            }

            // Act
            var result = _controller.CheckReferralCode(referralCode);

            // Assert
            if (expectedStatusCode == 200)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(new { Exists = true }, okResult.Value);
            }
            else if (expectedStatusCode == 404)
            {
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal(404, notFoundResult.StatusCode);
                Assert.Equal(new { Exists = false }, notFoundResult.Value);
            }
            else if (expectedStatusCode == 400)
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(400, badRequestResult.StatusCode);
                Assert.Equal("Referral code is required.", badRequestResult.Value);
            }
        }

        [Fact]
        public void GenerateReferralCode_ReturnsUniqueCodes()
        {
            // Arrange
            var codesToReturn = new List<string> { "CODE123", "CODE456", "CODE789" };
            _contextMock.SetupSequence(c => c.GenerateReferralCode(It.IsAny<string>()))
                        .Returns(codesToReturn[0])
                        .Returns(codesToReturn[1])
                        .Returns(codesToReturn[2]);

            // Act
            var result1 = _controller.GenerateReferralCode("user1") as OkObjectResult;
            var result2 = _controller.GenerateReferralCode("user1") as OkObjectResult;
            var result3 = _controller.GenerateReferralCode("user1") as OkObjectResult;

            var returnedCodes = new List<string>();
            if (result1 != null && result1.Value is ReferralCodeResponse response1) returnedCodes.Add(response1.ReferralCode);
            if (result2 != null && result2.Value is ReferralCodeResponse response2) returnedCodes.Add(response2.ReferralCode);
            if (result3 != null && result3.Value is ReferralCodeResponse response3) returnedCodes.Add(response3.ReferralCode);

            // Assert uniqueness
            Assert.Equal(3, returnedCodes.Count);
            Assert.True(returnedCodes.Distinct().Count() == returnedCodes.Count, "Generated codes are not unique.");
        }

    }
}