using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using VideoGameStore;
using VideoGameStore.Models;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SendGridClient _sendGridClient;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        var apiKey = configuration["SendGrid:ApiKey"];
        _sendGridClient = new SendGridClient(apiKey);
    }
    public async Task<ResultType> SendWelcomeEmail(User user)
    {
        try
        {
            var apiKey = "your_api_key"; // Ideally, inject this
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("videostoregamesofficial@gmail.com", "Video Game Store");
            var subject = "Congrats on registering";
            var to = new EmailAddress(user.email, user.username);
            var plainTextContent = $"Welcome {user.name}, to our site!";
            var htmlContent = $"<strong>Welcome {user.name}, to our site!</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
        
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Body.ReadAsStringAsync();
                _logger.LogError($"Failed to send email. Status code: {response.StatusCode}, Message: {errorMessage}");
                return new ResultType { Success = false, Message = $"Error sending welcome email: {errorMessage}" };
            }

            _logger.LogInformation("Welcome email sent successfully to {Email}", user.email);
            return new ResultType { Success = true, Message = "Welcome email sent successfully." };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending welcome email to {user.email}: {ex.Message}, StackTrace: {ex.StackTrace}");
            return new ResultType { Success = false, Message = $"Error sending welcome email: {ex.Message}" };
        }
    }

}

public class ResultType
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
