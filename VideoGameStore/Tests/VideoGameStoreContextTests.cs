using MySql.Data.MySqlClient;
using System;
using VideoGameStore.Models;
using Xunit;

namespace IntegrationTests.VideoGameStoreContextTests
{
    public class VideoGameStoreContextTests : IDisposable
    {
        private readonly VideoGameStoreContext _context;
        private readonly string _testConnectionString = "Server=localhost;Database=videogamestore;User=root;Password=root;";

        public VideoGameStoreContextTests()
        {
            _context = new VideoGameStoreContext(_testConnectionString);
        }
        
        [Fact]
        public void Connection_ShouldBeOpen()
        {
            using (var connection = new MySqlConnection(_testConnectionString))
            {
                connection.Open();
                Assert.True(connection.State == System.Data.ConnectionState.Open, "Connection should be open.");
            }
        }

        public void Dispose()
        {
        }
    }
}