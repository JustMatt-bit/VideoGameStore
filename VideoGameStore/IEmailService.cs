using VideoGameStore.Models;

namespace VideoGameStore;

public interface IEmailService
{
    Task<ResultType> SendWelcomeEmail(User user);
}