﻿using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

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
                        user.password = reader.GetString("password");
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

        public List<Product> GetSellableProducts()
        {
            List<Product> types = new List<Product>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT p.product_id, p.name, p.price, p.stock, p.description, p.release_date, p.being_sold, p.fk_game_type, gt.name AS game_type, p.fk_developer, d.name AS developer, p.fk_account " +
                    "FROM products p LEFT JOIN developers d ON  d.developer_id=p.fk_developer LEFT JOIN game_types gt ON  gt.game_type_id=p.fk_game_type WHERE p.being_sold=1", connection);
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

        public List<Product> GetUserProducts(string name)
        {
            List<Product> types = new List<Product>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT p.product_id, p.name, p.price, p.stock, p.description, p.release_date, p.being_sold, p.fk_game_type, gt.name AS game_type, p.fk_developer, d.name AS developer, p.fk_account " +
                    "FROM products p LEFT JOIN developers d ON  d.developer_id=p.fk_developer LEFT JOIN game_types gt ON  gt.game_type_id=p.fk_game_type WHERE p.fk_account=\""+name+"\"", connection);
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

        public bool RegisterUser(string username, string password, string name, string surname, string email, string phone, string refferal)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string hashedPassword = HashPassword(password);

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO accounts (username, password, name, surname, email, referal_code, phone, fk_user_type, fk_loyalty_tier) " +
                    "VALUES (@username, @password, @name, @surname, @email,@referal, @phone, 1, 1)",
                    connection);

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@referal", refferal);
                cmd.Parameters.AddWithValue("@phone", phone);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        
        private bool ValidatePassword(string enteredPassword, string storedPasswordHash)
        {
            
            string entryHash = HashPassword(enteredPassword);
       
                // Compare the stored hash with the hash of the entered password
            return entryHash.Equals(storedPasswordHash);
   
        }

        public bool Login(HttpContext httpContext, string username, string password)
        {
            // Get the user by username
            User user = GetUserByUsername(username);

            if (user != null)
            {
                // Validate the password
                if (ValidatePassword(password, user.password))
                {
                    // Password is valid
                    var token = $"YourSecretKey{username}";

                    // Set the authentication cookie using the provided HttpContext
                    SetAuthenticationCookie(httpContext, username, token); // Pass username and token

                    return true;
                }
            }

            // Authentication failed
            return false;
        }

        private void SetAuthenticationCookie(HttpContext httpContext, string username, string token)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddHours(1),
                Path = "/",
                HttpOnly = true,
                Secure = true, // Requires HTTPS
                SameSite = SameSiteMode.Lax
            };
            string cookieName = "AuthCookie"; // Include the username in the cookie name
            httpContext.Response.Cookies.Append(cookieName, token, cookieOptions);
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                string hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                // Convert byte array to a string
                return hash.Substring(0, 32);
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

        public bool UpdateUser(User updatedUser)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    StringBuilder sqlCommandBuilder = new StringBuilder("UPDATE accounts SET ");

                    // Check and add parameters only if the values are not null
                    if (!string.IsNullOrEmpty(updatedUser.name))
                    {
                        sqlCommandBuilder.Append("name=@name, ");
                    }
                    if (!string.IsNullOrEmpty(updatedUser.surname))
                    {
                        sqlCommandBuilder.Append("surname=@surname, ");
                    }
                    if (!string.IsNullOrEmpty(updatedUser.email))
                    {
                        sqlCommandBuilder.Append("email=@email, ");
                    }
                    if (!string.IsNullOrEmpty(updatedUser.phone))
                    {
                        sqlCommandBuilder.Append("phone=@phone, ");
                    }
                   
                        sqlCommandBuilder.Append("referal_code=@referal, ");
                    
                    if (!string.IsNullOrEmpty(updatedUser.password))
                    {
                        sqlCommandBuilder.Append("password=@password, ");
                    }

                    // Remove the trailing comma and space
                    sqlCommandBuilder.Length -= 2;

                    sqlCommandBuilder.Append(" WHERE username=@username");

                    MySqlCommand cmd = new MySqlCommand(sqlCommandBuilder.ToString(), connection);

                    // Add parameters based on the non-null values
                    if (!string.IsNullOrEmpty(updatedUser.name))
                    {
                        cmd.Parameters.AddWithValue("@name", updatedUser.name);
                    }
                    if (!string.IsNullOrEmpty(updatedUser.surname))
                    {
                        cmd.Parameters.AddWithValue("@surname", updatedUser.surname);
                    }
                    if (!string.IsNullOrEmpty(updatedUser.email))
                    {
                        cmd.Parameters.AddWithValue("@email", updatedUser.email);
                    }
                    if (!string.IsNullOrEmpty(updatedUser.phone))
                    {
                        cmd.Parameters.AddWithValue("@phone", updatedUser.phone);
                    }
             
                        cmd.Parameters.AddWithValue("@referal", updatedUser.referal_code);
                    
                    if (!string.IsNullOrEmpty(updatedUser.password))
                    {
                        // Hash the new password before updating
                        cmd.Parameters.AddWithValue("@password", HashPassword(updatedUser.password));
                    }

                    cmd.Parameters.AddWithValue("@username", updatedUser.username);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log, throw, etc.)
                return false;
            }
        }

        public List<Order> GetOrderHistoryByUsername(string username)
        {
            List<Order> orderHistory = new List<Order>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM orders WHERE fk_account = @username", connection);
                cmd.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderHistory.Add(new Order()
                        {
                            id = reader.GetInt32("order_id"),
                            creation_date = reader.GetDateTime("creation_date"),
                            completion_date = reader.GetDateTime("completion_date"),
                            price = reader.GetFloat("price"),
                            comment = reader.GetString("comment"),
                            parcel_price = reader.GetFloat("parcel_price"),
                            fk_account = reader.GetString("fk_account"),
                            fk_address = reader.IsDBNull(reader.GetOrdinal("fk_address"))
                                         ? (int?)null
                                         : reader.GetInt32("fk_address"),
                            fk_status = reader.GetInt32("fk_status"),
                            fk_discount = reader.IsDBNull(reader.GetOrdinal("fk_discount"))
                                          ? (int?)null
                                          : reader.GetInt32("fk_discount")
                        });
                    }
                }
                return orderHistory;
            }
        }
    }
}