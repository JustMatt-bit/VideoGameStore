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

    public async Task<ResultType> SendWelcomeEmail(User user)
    {
        try
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("videostoregamesofficial@gmail.com", "Video game store");
            var subject = "Congrats on registering";
            var to = new EmailAddress(user.email, user.username);
            var plainTextContent = "Welcome " + user.name + ", To our site";
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
