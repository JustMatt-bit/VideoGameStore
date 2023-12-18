using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Add this namespace
using VideoGameStore.Models;
public class EmailService
{

    private readonly ILogger<EmailService> _logger;

    public EmailService( ILogger<EmailService> logger)
    {
        _logger = logger;
    }
    public async Task<ResultType> SendEmailAsync(string userEmail, string subject, string body)
    {
        try
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("videogamestorektu@gmail.com", "Video game store");
            var to = new EmailAddress(userEmail);
            var plainTextContent = body;
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            _logger.LogInformation($"{subject} email sent successfully.");

            // Return a success result or appropriate data
            return new ResultType { Success = true, Message = $"{subject} email sent successfully." };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending {subject} email: {ex.Message}");

            // Return an error result or handle the error as needed
            return new ResultType { Success = false, Message = $"Error sending {subject} email: {ex.Message}" };
        }
    }
    public async Task<ResultType> SendWelcomeEmail(User user)
    {
        try
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("videogamestorektu@gmail.com", "Video game store");
            var subject = "Welcome " + user.name + ", To our site";
            var to = new EmailAddress(user.email, user.username);
            var plainTextContent = $"We are happy to see you registering and verifying your account on our site, {user.username}!!!";
            var htmlContent = "Welcome " + user.name + ", To our site";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            _logger.LogInformation("Welcome email sent successfully.");

            // Return a success result or appropriate data
            return new ResultType { Success = true, Message = "Welcome email sent successfully." };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending welcome email: {ex.Message}");

            // Return an error result or handle the error as needed
            return new ResultType { Success = false, Message = $"Error sending welcome email: {ex.Message}" };
        }
    }
}

public class ResultType
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
