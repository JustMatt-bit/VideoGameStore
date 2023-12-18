using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;

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
                    while (reader.Read())
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
                        user.loyalty_progress = reader.GetInt32("loyalty_progress");
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

        public void AddProductToCart(int orderID, int productID, float price)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                   "INSERT INTO carts (price, stock, fk_order, fk_product) " +
                   "VALUES (@price, 1, @fk_order, @fk_product)",
                   connection);

                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@fk_order", orderID);
                cmd.Parameters.AddWithValue("@fk_product", productID);

                int rowsAffected = cmd.ExecuteNonQuery();
            }
        }

        public List<int> TopPopularGenres()
        {
            List<int> topGenres = new List<int>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT product_genres.fk_genre,COUNT(product_genres.fk_genre)as countval FROM product_genres GROUP BY product_genres.fk_genre ORDER BY countval DESC LIMIT 5", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topGenres.Add(reader.GetInt32("fk_genre"));
                    }
                }
            }
            return topGenres;
        }

        public List<int> TopPopularGames()
        {
            List<int> topGames = new List<int>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT carts.fk_product,SUM(carts.stock)as sumval FROM carts INNER JOIN orders ON carts.fk_order=orders.order_id AND orders.fk_status=6 GROUP BY carts.fk_product ORDER BY sumval DESC LIMIT 10", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topGames.Add(reader.GetInt32("fk_product"));
                    }
                }
            }
            return topGames;
        }

        List<int> GetLeastPopularGames()
        {
            List<int> bottomGames = new List<int>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT products.product_id, COUNT(products.product_id) as countval FROM products LEFT JOIN carts ON carts.fk_product=products.product_id GROUP BY products.product_id ORDER BY countval LIMIT 10", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bottomGames.Add(reader.GetInt32("product_id"));
                    }
                }
            }
            return bottomGames;
        }

        public List<int> GetUserGenres(string username)
        {
            List<int> topGenres = new List<int>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT fk_genre FROM user_genres WHERE fk_account= \"" + username + "\"", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topGenres.Add(reader.GetInt32("fk_genre"));
                    }
                }
            }
            return topGenres;
        }

        public List<int> GetPopularGamesFromGenres(List<int> genres)
        {
            List<int> topGames = new List<int>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd;
                foreach (var genre in genres)
                {
                    cmd = new MySqlCommand(
                    "SELECT *,SUM(carts.stock)as sumval FROM product_genres INNER JOIN carts ON carts.fk_product=product_genres.fk_product AND product_genres.fk_genre=\"" + genre + "\" INNER JOIN orders ON carts.fk_order=orders.order_id AND orders.fk_status=6 GROUP BY product_genres.fk_product ORDER BY sumval DESC LIMIT 5", connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topGames.Add(reader.GetInt32("fk_product"));
                        }
                    }
                }
            }
            topGames = topGames.Distinct().ToList();
            return topGames;
        }



        public List<Product> GetRecommendations(string name)
        {
            List<Product> products = new List<Product>();
            List<int> userGenres = GetUserGenres(name);
            List<int> topGenres = TopPopularGenres();
            List<int> topPopularGames = TopPopularGames();
            List<int> popularGameFromGenre = new List<int>();
            //if uer has no favorite genres
            if (userGenres.Count == 0) {
                
                popularGameFromGenre.AddRange(GetPopularGamesFromGenres(topGenres));
            }
            else
            {
                popularGameFromGenre.AddRange(GetPopularGamesFromGenres(topGenres));
                popularGameFromGenre.AddRange(GetPopularGamesFromGenres(userGenres));
            }
            popularGameFromGenre.AddRange(topPopularGames);
            List<int> leastPopularGame = GetLeastPopularGames();
            popularGameFromGenre.AddRange(leastPopularGame);
            popularGameFromGenre = popularGameFromGenre.Distinct().ToList();
            //get 5 random games from generated game array (if generated array lenght is less than or equal 5, than return all array
            List<int> recommendedGameIds = new List<int>();
            if (popularGameFromGenre.Count > 5)
            {
                int i = 5;
                Random rnd = new Random();
                while (0 < i)
                {
                    int randIndex = rnd.Next(0, popularGameFromGenre.Count);
                    var item = popularGameFromGenre[randIndex];
                    popularGameFromGenre.RemoveAt(randIndex);
                    recommendedGameIds.Add(item);
                    i--;
                }
                products = GetProductsByIds(recommendedGameIds);
            }
            else
            {
                products = GetProductsByIds(popularGameFromGenre);
            }
            return products;
            
        }


        public List<Product> GetProductsByIds(List<int> ids)
        {
            List<Product> product = new List<Product>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd;
                foreach(var id in ids){
                    cmd = new MySqlCommand(
                        "SELECT p.product_id, p.name, p.price, p.stock, p.description, p.release_date, p.being_sold, p.fk_game_type, gt.name AS game_type, p.fk_developer, d.name AS developer, p.fk_account, p.image " +
                        "FROM products p LEFT JOIN developers d ON  d.developer_id=p.fk_developer LEFT JOIN game_types gt ON  gt.game_type_id=p.fk_game_type WHERE p.product_id=\"" + id + "\"", connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            product.Add(new Product()
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
                                image = reader.GetString("image")
                            });
                        }

                    }
                }
            }
            return product;
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



        public bool GenreExists(string name)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT COUNT(*) as count FROM genres WHERE name=\"" + name + "\"", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return reader.GetInt32("count") != 0;
                    
                }
            }
        }

        public bool CreateGenre(string name, string description)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO genres (name, description) " +
                    "VALUES (@name, @description)",
                    connection);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public List<Product> GetUserProducts(string name)
        {
            List<Product> types = new List<Product>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT p.product_id, p.name, p.price, p.stock, p.description, p.release_date, p.being_sold, p.fk_game_type, gt.name AS game_type, p.fk_developer, d.name AS developer, p.fk_account, p.image " +
                    "FROM products p LEFT JOIN developers d ON  d.developer_id=p.fk_developer LEFT JOIN game_types gt ON  gt.game_type_id=p.fk_game_type WHERE p.fk_account=\"" + name + "\"", connection);
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
                            fk_account = reader.GetString("fk_account"),
                            image = reader.GetString("image")
                        });
                    }
                }
                return types;
            }
        }


        public List<GameType> GetGameTypes()
        {
            List<GameType> gameTypes = new List<GameType>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM game_types", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        gameTypes.Add(new GameType()
                        {
                            id = reader.GetInt32("game_type_id"),
                            name = reader.GetString("name")
                        }
                        );
                    }
                }
            }
            return gameTypes;
        }

        public bool GenresProductConnection(int id, List<Genre> genres)
        {
            using (MySqlConnection connection = GetConnection())
            {
                int rowsAffected = 0;
                connection.Open();
                foreach (Genre genre in genres)
                {
                    MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO product_genres (fk_genre, fk_product) " +
                    "VALUES (@genre, @id)",
                    connection);

                    cmd.Parameters.AddWithValue("@genre", genre.id);
                    cmd.Parameters.AddWithValue("@id", id);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                
                return rowsAffected > 0;
            }
        }

        public int CreateDeveloper(string name, string country)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO developers (name, country) " +
                    "VALUES (@name, @country)",
                    connection);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@country", country);

                int rowsAffected = cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(
                    "SELECT LAST_INSERT_ID()",
                    connection);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }

        public List<Developer> GetDevelopers()
        {
            List<Developer> developers = new List<Developer>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM developers", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        developers.Add(new Developer()
                        {
                            id = reader.GetInt32("developer_id"),
                            name = reader.GetString("name"),
                            country = reader.GetString("country")
                        }
                        );
                    }
                }
            }
            return developers;
        }
        public List<int> GetProductGenres(int id)
        {
            List<int> genres = new List<int>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM product_genres WHERE fk_product=\"" + id + "\"", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        genres.Add(reader.GetInt32("fk_genre"));
                    }
                }
            }
            return genres;

        }

        public List<Genre> GetGenres()
        {
            List<Genre> genres = new List<Genre>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM genres", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        genres.Add(new Genre()
                        {
                            id = reader.GetInt32("genre_id"),
                            name = reader.GetString("name"),
                            description = reader.GetString("description")
                        }
                        );
                    }
                }
            }
            return genres;

        }
        public bool DeleteGenres(List<Genre> genres)
        {
            bool deleted = false;
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                foreach (Genre genre in genres) {
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM genres WHERE genre_id=\"" + genre.id + "\"", connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            deleted = true;
                        }
                    }
                }
                
            }
            return deleted;

        }
        public bool DeleteGenresProductConnection(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM product_genres WHERE fk_product=\"" + id + "\"", connection);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                

            }

        }
        public bool DeleteProductIfNotInUse(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT COUNT(*) FROM carts WHERE fk_product=\"" + id + "\"", connection);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                int rowsAffected = 0;
                if (count == 0)
                {
                    cmd = new MySqlCommand(
                        "DELETE FROM product_genres WHERE fk_product=\"" + id + "\"", connection);
                    rowsAffected = cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand(
                        "DELETE FROM products WHERE product_id=\"" + id + "\"", connection);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                else
                {

                    cmd = new MySqlCommand("UPDATE products SET being_sold=0 WHERE product_id=\"" + id + "\"", connection);
                    int rows = cmd.ExecuteNonQuery();
                }
                return rowsAffected > 0;


            }

        }

        public bool DeleteDeveloper(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT COUNT(*) FROM products WHERE fk_developer=\"" + id + "\"", connection);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                int rowsAffected = 0;
                if (count == 0)
                {
                    cmd = new MySqlCommand(
                        "DELETE FROM developers WHERE developer_id=\"" + id + "\"", connection);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                return rowsAffected > 0;


            }

        }
        public bool ChangeProductImage(int id, string name)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE products SET image=@image WHERE product_id=@id",
                    connection);

                cmd.Parameters.AddWithValue("@image", name);
                cmd.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public bool UpdateProduct(Product product)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                StringBuilder sqlCommandBuilder = new StringBuilder("UPDATE products SET ");

                sqlCommandBuilder.Append("name=@name, ");
                sqlCommandBuilder.Append("price=@price, ");
                sqlCommandBuilder.Append("stock=@stock, ");
                sqlCommandBuilder.Append("description=@description, ");
                sqlCommandBuilder.Append("release_date=@release_date, ");
                sqlCommandBuilder.Append("being_sold=@being_sold, ");
                sqlCommandBuilder.Append("fk_game_type=@fk_game_type, ");
                sqlCommandBuilder.Append("fk_developer=@fk_developer, ");
                sqlCommandBuilder.Append("fk_account=@fk_account, ");

                if (!string.IsNullOrEmpty(product.image))
                {
                    sqlCommandBuilder.Append("image=@image, ");
                }
                sqlCommandBuilder.Length -= 2;

                sqlCommandBuilder.Append(" WHERE product_id=@id");

                MySqlCommand cmd = new MySqlCommand(sqlCommandBuilder.ToString(), connection);
                

                cmd.Parameters.AddWithValue("@name", product.name);
                cmd.Parameters.AddWithValue("@price", product.price);
                cmd.Parameters.AddWithValue("@stock", product.stock);
                cmd.Parameters.AddWithValue("@description", product.description);
                cmd.Parameters.AddWithValue("@release_date", product.release_date);
                cmd.Parameters.AddWithValue("@being_sold", product.being_sold);
                cmd.Parameters.AddWithValue("@fk_game_type", product.fk_game_type);
                cmd.Parameters.AddWithValue("@fk_developer", product.fk_developer);
                cmd.Parameters.AddWithValue("@fk_account", product.fk_account);
                if (!string.IsNullOrEmpty(product.image))
                {
                    cmd.Parameters.AddWithValue("@image", product.image);
                }
                cmd.Parameters.AddWithValue("@id", product.id);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }


        public int CreateProduct(Product product)
        {

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO products (name, price, stock, description, release_date, being_sold, fk_game_type, fk_developer, fk_account, image) " +
                    "VALUES (@name, @price, @stock, @description, @release_date, @being_sold, @fk_game_type, @fk_developer, @fk_account, @image)",
                    connection);

                cmd.Parameters.AddWithValue("@name", product.name);
                cmd.Parameters.AddWithValue("@price", product.price);
                cmd.Parameters.AddWithValue("@stock", product.stock);
                cmd.Parameters.AddWithValue("@description", product.description);
                cmd.Parameters.AddWithValue("@release_date", product.release_date);
                cmd.Parameters.AddWithValue("@being_sold", product.being_sold);
                cmd.Parameters.AddWithValue("@fk_game_type", product.fk_game_type);
                cmd.Parameters.AddWithValue("@fk_developer", product.fk_developer);
                cmd.Parameters.AddWithValue("@fk_account", product.fk_account);
                cmd.Parameters.AddWithValue("@image", product.image);

                int rowsAffected = cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(
                    "SELECT LAST_INSERT_ID()",
                    connection);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }



        public Product GetProduct(int id)
        {
            Product product = new Product();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT p.product_id, p.name, p.price, p.stock, p.description, p.release_date, p.being_sold, p.fk_game_type, gt.name AS game_type, p.fk_developer, d.name AS developer, p.fk_account, p.image " +
                    "FROM products p LEFT JOIN developers d ON  d.developer_id=p.fk_developer LEFT JOIN game_types gt ON  gt.game_type_id=p.fk_game_type WHERE p.product_id=\"" + id + "\"", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    product = new Product()
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
                        image = reader.GetString("image")
                    };

                }
            }
            return product;
        }



        public bool RegisterUser(string username, string password, string name, string surname, string email, string phone, string refferal)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string hashedPassword = HashPassword(password);

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO accounts (username, password, name, surname, email, phone, fk_user_type, fk_loyalty_tier) " +
                    "VALUES (@username, @password, @name, @surname, @email, @phone, 1, 1)",
                    connection);

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0 && CheckReferralCodeExists(refferal))
                {
                    // User registered successfully and a referral code was used
                    CreateDiscountForUser(username);
                }

                return rowsAffected > 0;
            }
        }

        private void CreateDiscountForUser(string username)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO discounts (valid_from, valid_to, percent, fk_account) " +
                    "VALUES (@validFrom, @validTo, @percent, (SELECT username FROM accounts WHERE username = @username))",
                    connection);

                cmd.Parameters.AddWithValue("@validFrom", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@validTo", DateTime.UtcNow.AddMonths(1));
                cmd.Parameters.AddWithValue("@percent", 10.0); // 10 percent discount
                cmd.Parameters.AddWithValue("@username", username);

                cmd.ExecuteNonQuery();
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

        public bool CreateFeedbackForProduct(int productId, Feedback feedback, string username)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Prepare the INSERT statement to add new feedback
                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO feedback (date, text, rating, rating_count, flagged, fk_account, fk_product) " +
                    "VALUES (@date, @text, @rating, @ratingCount, @flagged, @accountName, @productId)", connection);

                // Set the parameters
                cmd.Parameters.AddWithValue("@date", feedback.date);
                cmd.Parameters.AddWithValue("@text", feedback.text);
                cmd.Parameters.AddWithValue("@rating", feedback.rating);
                cmd.Parameters.AddWithValue("@ratingCount", feedback.rating_count);
                cmd.Parameters.AddWithValue("@flagged", feedback.is_flagged);
                cmd.Parameters.AddWithValue("@accountName", username);
                cmd.Parameters.AddWithValue("@productId", productId);

                // Execute the INSERT statement
                int rowsAffected = cmd.ExecuteNonQuery();

                // Return true if one row was affected, otherwise false
                return rowsAffected == 1;
            }
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

        public List<User> GetTopUsersByLoyaltyProgress()
        {
            List<User> topUsers = new List<User>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM accounts ORDER BY loyalty_progress DESC LIMIT 10", connection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topUsers.Add(new User
                        {
                            username = reader.GetString("username"),
                            name = reader.GetString("name"),
                            surname = reader.GetString("surname"),
                            password = reader.GetString("password"),
                            email = reader.GetString("email"),
                            phone = reader.GetString("phone"),
                            referal_code = reader.IsDBNull(reader.GetOrdinal("referal_code"))
                                                ? (string?)null
                                                : reader.GetString("referal_code"),
                            creation_date = reader.GetDateTime("creation_date"),
                            loyalty_progress = reader.GetInt32("loyalty_progress"),
                            fk_user_type = reader.GetInt32("fk_user_type"),
                            fk_loyalty_tier = reader.GetInt32("fk_loyalty_tier")
                        });
                    }
                }
            }
            return topUsers;
        }

        public int GetUserPositionByUsername(string username)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) + 1 as position FROM accounts WHERE loyalty_progress > (SELECT loyalty_progress FROM accounts WHERE username = @username)", connection);
                cmd.Parameters.AddWithValue("@username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32("position");
                    }
                    return -1;
                }
            }
        }

        public LoyaltyTier GetUserLoyaltyTier(string username)
        {
            LoyaltyTier currentTier = new LoyaltyTier();
            using (var connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT t.* FROM loyalty_tiers t JOIN accounts a ON t.tier_id = a.fk_loyalty_tier WHERE a.username = @username", connection);
                cmd.Parameters.AddWithValue("@username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentTier = new LoyaltyTier
                        {
                            TierId = reader.GetInt32("tier_id"),
                            Name = reader.GetString("name"),
                            PointsFrom = reader.GetInt32("points_from"),
                            PointsTo = reader.GetInt32("points_to"),
                            Description = reader.GetString("description"),
                            DiscountCoefficient = reader.GetDouble("discount_coeficient")
                        };
                    }
                }
            }

            return currentTier;
        }

        public LoyaltyTier GetNextLoyaltyTier(int currentTierId)
        {
            LoyaltyTier nextTier = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM loyalty_tiers WHERE tier_id = @tierId + 1", connection);
                cmd.Parameters.AddWithValue("@tierId", currentTierId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nextTier = new LoyaltyTier
                        {
                            TierId = reader.GetInt32("tier_id"),
                            Name = reader.GetString("name"),
                            PointsFrom = reader.GetInt32("points_from"),
                            PointsTo = reader.GetInt32("points_to"),
                            Description = reader.GetString("description"),
                            DiscountCoefficient = reader.GetDouble("discount_coeficient")
                        };
                    }
                }
            }

            return nextTier;
        }

        public bool CheckReferralCodeExists(string referralCode)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var cmd = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE referal_code = @referralCode", connection);
                cmd.Parameters.AddWithValue("@referralCode", referralCode);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public string GenerateReferralCode(string username)
        {
            var referralCode = GenerateUniqueCode();
            using (var connection = GetConnection())
            {
                connection.Open();
                var cmd = new MySqlCommand("UPDATE accounts SET referal_code = @referralCode WHERE username = @username", connection);
                cmd.Parameters.AddWithValue("@referralCode", referralCode);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
            }
            return referralCode;
        }

        private string GenerateUniqueCode()
        {
            // Simple example of generating a 10-character alphanumeric string
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var uniqueCode = new string(Enumerable.Repeat(characters, 10)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
            return uniqueCode;
        }

        public IEnumerable<Discount> GetDiscountsByUsername(string username)
        {
            List<Discount> discounts = new List<Discount>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM discounts WHERE fk_account = @username AND valid_to >= CURDATE()", connection);
                cmd.Parameters.AddWithValue("@username", username);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        discounts.Add(new Discount()
                        {
                            DiscountId = reader.GetInt32("discount_id"),
                            ValidFrom = reader.GetDateTime("valid_from"),
                            ValidTo = reader.GetDateTime("valid_to"),
                            Percent = reader.GetDouble("percent"),
                            FkAccount = reader.GetString("fk_account")
                        });
                    }
                }
            }
            return discounts;
        }

        public bool ApplyDiscountToUser(string username, int discountId)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Assuming you have a mechanism to associate discounts with orders or carts
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE orders SET fk_discount = @discountId WHERE fk_account = @username AND order_id = (SELECT MAX(order_id) FROM orders WHERE fk_account = @username)", connection);
                cmd.Parameters.AddWithValue("@discountId", discountId);
                cmd.Parameters.AddWithValue("@username", username);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }


    }
}