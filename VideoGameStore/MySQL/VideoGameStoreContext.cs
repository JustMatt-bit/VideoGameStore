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
        public List<Feedback> GetFeedbacksForProduct(int productId)
        {
            List<Feedback> feedbacks = new List<Feedback>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "SELECT f.feedback_id, f.date, f.text, f.rating, f.rating_count, f.flagged, f.fk_account, f.fk_product" +
                    "FROM feedback f LEFT JOIN products p ON p.product_id=f.fk_product WHERE f.fk_product=@productId", connection);
                cmd.Parameters.AddWithValue("@productId", productId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        feedbacks.Add(new Feedback()
                        {
                            id = reader.GetInt32("feedback_id"),
                            date = reader.GetDateTime("date"),
                            text = reader.GetString("text"),
                            rating = reader.GetFloat("rating"),
                            rating_count = reader.GetInt32("rating_count"),
                            is_flagged = reader.GetBoolean("flagged"),
                            account_name = reader.GetString("account_name"),
                            fk_product = reader.GetInt32("fk_product")
                        });
                    }
                }
            }
            return feedbacks;
        }
    }
}