/*
 * Service class for interacting with MongoDB.
 * Responsible for establishing a connection to the database
 * using the configuration settings.
 */


using MongoDB.Driver;

namespace LumaShopAPI.Services.Database
{
    public class MongodbService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase _database;

        public MongodbService(IConfiguration configuration)
        {
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString("MongodbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase Database => _database;
    }
}
