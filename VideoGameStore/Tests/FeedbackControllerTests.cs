using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VideoGameStore.Controllers;
using VideoGameStore.Models;
using Xunit;

namespace VideoGameStore.Tests
{
    public class FeedbackControllerTests
    {
        private readonly Mock<ILogger<FeedbackController>> _loggerMock;
        private readonly Mock<IVideoGameStoreContext> _contextMock;
        private readonly FeedbackController _controller;

        public FeedbackControllerTests()
        {
            _loggerMock = new Mock<ILogger<FeedbackController>>();
            _contextMock = new Mock<IVideoGameStoreContext>();
            _controller = new FeedbackController(_loggerMock.Object, _contextMock.Object);
        }

        [Fact]
        public void Get_ReturnsOkResult_WithFeedbackList()
        {
            // Arrange
            int productId = 1;
            var feedbackList = new List<Feedback>
            {
                new Feedback { id = 1, text = "Great game!", rating = 4.5f, fk_product = productId },
                new Feedback { id = 2, text = "Not bad", rating = 3.0f, fk_product = productId }
            };

            _contextMock.Setup(context => context.GetFeedbackForProduct(productId)).Returns(feedbackList);

            // Act
            var result = _controller.Get(productId).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(feedbackList, result.Value);
        }

        [Fact]
        public void Get_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int productId = 1;
            _contextMock.Setup(context => context.GetFeedbackForProduct(productId)).Throws(new Exception("Database error"));

            // Act
            var result = _controller.Get(productId).Result as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Internal server error", result.Value);
        }

        [Fact]
        public void CreateFeedback_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            int productId = 1;
            var feedback = new Feedback { text = "Good game!", rating = 5.0f };
            var request = new FeedbackController.FeedbackRequestModel { Feedback = feedback, Username = "testUser" };

            _contextMock.Setup(context => context.CreateFeedbackForProduct(productId, feedback, "testUser")).Returns(true);

            // Act
            var result = _controller.CreateFeedback(productId, request) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void CreateFeedback_ReturnsBadRequest_OnFailure()
        {
            // Arrange
            int productId = 1;
            var feedback = new Feedback { text = "Good game!", rating = 5.0f };
            var request = new FeedbackController.FeedbackRequestModel { Feedback = feedback, Username = "testUser" };

            _contextMock.Setup(context => context.CreateFeedbackForProduct(productId, feedback, "testUser")).Returns(false);

            // Act
            var result = _controller.CreateFeedback(productId, request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Failed to save feedback", result.Value);
        }

        [Fact]
        public void ReportFeedback_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            int feedbackId = 1;
            _contextMock.Setup(context => context.ReportFeedback(feedbackId)).Returns(true);

            // Act
            var result = _controller.ReportFeedback(feedbackId) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void ReportFeedback_ReturnsBadRequest_OnFailure()
        {
            // Arrange
            int feedbackId = 1;
            _contextMock.Setup(context => context.ReportFeedback(feedbackId)).Returns(false);

            // Act
            var result = _controller.ReportFeedback(feedbackId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Failed to report feedback", result.Value);
        }

        [Fact]
        public void RateFeedback_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            int feedbackId = 1;
            int newRating = 5;
            _contextMock.Setup(context => context.RateFeedback(feedbackId, newRating)).Returns(true);

            // Act
            var result = _controller.RateFeedback(feedbackId, newRating) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void RateFeedback_ReturnsBadRequest_OnFailure()
        {
            // Arrange
            int feedbackId = 1;
            int newRating = 3;
            _contextMock.Setup(context => context.RateFeedback(feedbackId, newRating)).Returns(false);

            // Act
            var result = _controller.RateFeedback(feedbackId, newRating) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Failed to update rating", result.Value);
        }


    }
}
