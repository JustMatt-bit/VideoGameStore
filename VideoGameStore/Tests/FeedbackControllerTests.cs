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

        ///    - Verifies that the Get method returns an OkObjectResult with a list of feedback for a given product ID.
        ///    - Mocks the IVideoGameStoreContext to return a predefined list of feedback.
        ///    - Asserts that the result is not null, has a status code of 200, and contains the expected feedback list.
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


        ///    - Verifies that the Get method returns an ObjectResult with a status code of 500 when an exception occurs.
        ///    - Mocks the IVideoGameStoreContext to throw an exception.
        ///    - Asserts that the result is not null, has a status code of 500, and contains the expected error message.
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


        ///    - Verifies that the CreateFeedback method returns an OkResult when feedback is successfully created.
        ///    - Mocks the IVideoGameStoreContext to return true for feedback creation.
        ///    - Asserts that the result is not null and has a status code of 200.
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


        ///    - Verifies that the CreateFeedback method returns a BadRequestObjectResult when feedback creation fails.
        ///    - Mocks the IVideoGameStoreContext to return false for feedback creation.
        ///    - Asserts that the result is not null, has a status code of 400, and contains the expected error message.
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


        ///    - Verifies that the ReportFeedback method returns an OkResult when feedback is successfully reported.
        ///    - Mocks the IVideoGameStoreContext to return true for feedback reporting.
        ///    - Asserts that the result is not null and has a status code of 200.
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


        ///    - Verifies that the ReportFeedback method returns a BadRequestObjectResult when feedback reporting fails.
        ///    - Mocks the IVideoGameStoreContext to return false for feedback reporting.
        ///    - Asserts that the result is not null, has a status code of 400, and contains the expected error message.
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


        ///    - Verifies that the RateFeedback method returns an OkResult when feedback rating is successfully updated.
        ///    - Mocks the IVideoGameStoreContext to return true for feedback rating update.
        ///    - Asserts that the result is not null and has a status code of 200.
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


        ///    - Verifies that the RateFeedback method returns a BadRequestObjectResult when feedback rating update fails.
        ///    - Mocks the IVideoGameStoreContext to return false for feedback rating update.
        ///    - Asserts that the result is not null, has a status code of 400, and contains the expected error message.
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
