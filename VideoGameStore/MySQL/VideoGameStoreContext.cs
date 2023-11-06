using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace VideoGameStore.Models
{
    public class VideoGameStoreContext
    {
        public string ConnectionString { get; set; }

        public VideoGameStoreContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public User GetUserByUsername(string username)
        {
            User user = new User();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM accounts WHERE username=@user", connection);
                cmd.Parameters.AddWithValue("@user", username);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read()) 
                    {
                        user.name = reader.GetString("name");
                        user.surname = reader.GetString("surname");
                        user.email = reader.GetString("email");
                        user.phone = reader.GetString("phone");
                        user.referal_code = reader.IsDBNull(reader.GetOrdinal("referal_code"))
                                            ? (string?)null
                                            : reader.GetString("referal_code");
                        user.creation_date = reader.GetDateTime("creation_date");
                        user.fk_user_type = reader.GetInt32("fk_user_type");
                        user.fk_loyalty_tier = reader.GetInt32("fk_loyalty_tier");
                    }
                }
                return user;
            }
        }
        public User GetAdressesByUsername(string username)
        {
            User user = new User();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM addresses WHERE username=@user", connection);
                cmd.Parameters.AddWithValue("@user", username);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.name = reader.GetString("name");
                        user.surname = reader.GetString("surname");
                        user.email = reader.GetString("email");
                        user.phone = reader.GetString("phone");
                        user.referal_code = reader.IsDBNull(reader.GetOrdinal("referal_code"))
                                            ? (string?)null
                                            : reader.GetString("referal_code");
                        user.creation_date = reader.GetDateTime("creation_date");
                        user.fk_user_type = reader.GetInt32("fk_user_type");
                        user.fk_loyalty_tier = reader.GetInt32("fk_loyalty_tier");
                    }
                }
                return user;
            }
        }

        public List<Developer> GetAllDevelopers()
        {
            List<Developer> developers = new List<Developer>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM developers", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        developers.Add(new Developer()
                        {
                            id = reader.GetInt32("developer_id"),
                            name = reader.GetString("name"),
                            country = reader.GetString("description"),
                        });
                    }
                }
                return developers;
            }
        }

        public List<GameType> GetAllGameTypes()
        {
            List<GameType> types = new List<GameType>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM game_types", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(new GameType()
                        {
                            id = reader.GetInt32("game_type_id"),
                            name = reader.GetString("name")
                        });
                    }
                }
                return types;
            }
        }

        public List<Order> GetOrdersByUser(string username)
        {
            List<Order> orders = new List<Order>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    string.Format("SELECT * FROM orders WHERE fk_account=\"{0}\"", username), connection);
                Console.WriteLine(cmd.CommandText);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order()
                        {
                            id = reader.GetInt32("order_id"),
                            creation_date = reader.GetDateTime("creation_date"),
                            completion_date = reader.GetDateTime("completion_date"),
                            price = reader.GetFloat("price"),
                            comment = reader.GetString("comment"),
                            parcel_price = reader.GetFloat("parcel_price"),
                            fk_account = reader.GetString("fk_account"),
                            fk_status = reader.GetInt32("fk_status"),
                            fk_address = reader.IsDBNull(reader.GetOrdinal("fk_address"))
                                         ? (int?)null
                                         : reader.GetInt32("fk_address"),
                            fk_discount = reader.IsDBNull(reader.GetOrdinal("fk_discount"))
                                         ? (int?)null
                                         : reader.GetInt32("fk_discount")
                        });
                    }
                }
                Console.WriteLine(orders.Count);
                return orders;
            }
        }

        public List<Product> GetCartItemsByUser(string username)
        {
            var userOrders = GetOrdersByUser(username);
            var orderBeingBuilt = userOrders[userOrders.Count - 1];
            List<Product> cart_items = new List<Product>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT p.product_id, p.name, p.price, p.stock, p.description, p.release_date, p.being_sold, p.fk_game_type, gt.name AS game_type, p.fk_developer, d.name AS developer, p.fk_account, c.stock AS units_in_cart FROM products p LEFT JOIN developers d ON d.developer_id = p.fk_developer LEFT JOIN game_types gt ON gt.game_type_id = p.fk_game_type JOIN carts c ON c.fk_product = p.product_id WHERE c.fk_order = @orderID;", connection);
                cmd.Parameters.AddWithValue("@orderID", orderBeingBuilt.id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cart_items.Add(new Product()
                        {
                            id = reader.GetInt32("product_id"),
                            name = reader.GetString("name"),
                            price = reader.GetFloat("price"),
                            stock = reader.GetInt32("stock"),
                            description = reader.GetString("description"),
                            release_date = reader.GetDateTime("release_date"),
                            being_sold = reader.GetBoolean("being_sold"),
                            fk_game_type = reader.GetInt32("fk_game_type"),
                            game_type_name = reader.GetString("game_type"),
                            fk_developer = reader.GetInt32("fk_developer"),
                            developer_name = reader.GetString("developer"),
                            fk_account = reader.GetString("fk_account"),
                            units_in_cart = reader.GetInt32("units_in_cart"),
                        });
                    }
                }
                return cart_items;
            }
        }
        public void UpdateCartStock(int orderID, int productID, int newValue)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE carts SET stock =@newVal WHERE fk_order=@orderID AND fk_product=@productID;", connection);
                cmd.Parameters.AddWithValue("@newVal", newValue);
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@productID", productID);

                cmd.ExecuteNonQuery();
            }
        }
        public List<Product> GetAllProducts()
        {
            List<Product> types = new List<Product>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT p.product_id, p.name, p.price, p.stock, p.description, p.release_date, p.being_sold, p.fk_game_type, gt.name AS game_type, p.fk_developer, d.name AS developer, p.fk_account " +
                    "FROM products p LEFT JOIN developers d ON d.developer_id=p.fk_developer LEFT JOIN game_types gt ON gt.game_type_id=p.fk_game_type", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(new Product()
                        {
                            id = reader.GetInt32("product_id"),
                            name = reader.GetString("name"),
                            price = reader.GetFloat("price"),
                            stock = reader.GetInt32("stock"),
                            description = reader.GetString("description"),
                            release_date = reader.GetDateTime("release_date"),
                            being_sold = reader.GetBoolean("being_sold"),
                            fk_game_type = reader.GetInt32("fk_game_type"),
                            game_type_name = reader.GetString("game_type"),
                            fk_developer = reader.GetInt32("fk_developer"),
                            developer_name = reader.GetString("developer"),
                            fk_account = reader.GetString("fk_account")
                        });
                    }
                }
                return types;
            }
        }
        public List<Feedback> GetFeedbackForProduct(int productId)
        {
            List<Feedback> feedback = new List<Feedback>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "SELECT f.feedback_id, f.date, f.text, f.rating, f.rating_count, f.flagged, f.fk_account, f.fk_product " +
                    "FROM feedback f LEFT JOIN products p ON p.product_id=f.fk_product WHERE f.fk_product=@productId", connection);
                cmd.Parameters.AddWithValue("@productId", productId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        feedback.Add(new Feedback()
                        {
                            id = reader.GetInt32("feedback_id"),
                            date = reader.GetDateTime("date"),
                            text = reader.GetString("text"),
                            rating = reader.GetFloat("rating"),
                            rating_count = reader.GetInt32("rating_count"),
                            is_flagged = reader.GetBoolean("flagged"),
                            account_name = reader.IsDBNull(reader.GetOrdinal("fk_account"))
                                         ? (string?)null
                                         : reader.GetString("fk_account"),
                            fk_product = reader.GetInt32("fk_product")
                        });
                    }
                }
            }
            return feedback;
        }
    }
}