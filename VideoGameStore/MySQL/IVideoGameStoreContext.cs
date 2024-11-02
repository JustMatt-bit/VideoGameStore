using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Models;

namespace VideoGameStore.Models
{
    public interface IVideoGameStoreContext
    {
        Order GetOrderByID(int id);
        string GetStatusById(int statusId);
        void AddAddressToUsername(Address address);
        bool DeactivateAccount(string username);
        User GetUserByUsername(string username);
        bool Login(HttpContext httpContext, string username, string password);
        List<Address> GetAddressesByUsername(string username);
        List<Developer> GetAllDevelopers();
        List<GameType> GetAllGameTypes();
        void CreateNewBuildOrderFromOrderID(int orderID);
        void CreateNewBuildOrderFromUsername(string username);
        void UpdateOrderStatus(int orderID, int statusID);
        void UpdateOrderShipping(int orderID, float price);
        void UpdateOrderAddressByPostalCode(int orderID, string postalCode);
        void UpdateOrderPrice(int orderID, float price);
        void DecrementItem(int id, int amount);
        List<Order> GetOrdersByUser(string username);
        List<Product> GetCartItemsByUser(string username);
        void UpdateCartStock(int orderID, int productID, int newValue);
        void AddProductToCart(int orderID, int productID, float price);
        List<int> TopPopularGenres();
        List<int> TopPopularGames();
        List<int> GetLeastPopularGames();
        List<int> GetUserGenres(string username);
        List<int> GetPopularGamesFromGenres(List<int> genres);
        List<Product> GetRecommendations(string name);
        List<Product> GetProductsByIds(List<int> ids);
        List<Product> GetAllProducts();
        List<Product> GetSellableProducts();
        bool GenreExists(string name);
        bool CreateGenre(string name, string description);
        List<Product> GetUserProducts(string name);
        List<GameType> GetGameTypes();
        bool GenresProductConnection(int id, List<Genre> genres);
        int CreateDeveloper(string name, string country);
        List<Developer> GetDevelopers();
        List<int> GetProductGenres(int id);
        List<Genre> GetGenres();
        bool DeleteGenres(List<Genre> genres);
        bool DeleteGenresProductConnection(int id);
        bool DeleteProductIfNotInUse(int id);
        bool DeleteDeveloper(int id);
        bool ChangeProductImage(int id, string name);
        bool UpdateProduct(Product product);
        int CreateProduct(Product product);
        Product GetProduct(int id);
        bool RegisterUser(string username, string password, string name, string surname, string email, string phone, string refferal);
        List<Feedback> GetFeedbackForProduct(int productId);
        bool CreateFeedbackForProduct(int productId, Feedback feedback, string username);
        bool ReportFeedback(int feedbackId);
        bool RateFeedback(int feedbackId, int newRating);
        bool FeedbackExists(int feedbackId);
        bool UpdateUser(User updatedUser);
        List<Order> GetOrderHistoryByUsername(string username);
        List<Order> GetAllOrders();
        List<User> GetTopUsersByLoyaltyProgress();
        int GetUserPositionByUsername(string username);
        LoyaltyTier GetUserLoyaltyTier(string username);
        LoyaltyTier GetNextLoyaltyTier(int currentTierId);
        bool CheckReferralCodeExists(string referralCode);
        string GenerateReferralCode(string username);
        IEnumerable<Discount> GetDiscountsByUsername(string username);
        bool ApplyDiscountToUser(string username, int discountId);
        void UpdateUserLoyaltyProgress(string username, double loyaltyPoints);
        bool ApplyDiscountToOrder(int orderId, int discountId);
    }
}
