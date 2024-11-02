using VideoGameStore.Models;

namespace VideoGameStore.Models
{
    public interface IVideoGameStoreContext
    {
        Order GetOrderByID(int id);
        string GetStatusById(int statusId);
        List<Feedback> GetFeedbackForProduct(int productId);
        bool CreateFeedbackForProduct(int productId, Feedback feedback, string username);
        bool ReportFeedback(int feedbackId);
        bool RateFeedback(int feedbackId, int newRating);
        bool FeedbackExists(int feedbackId);

    }
}
