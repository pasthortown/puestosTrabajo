using Backend.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Backend.Infrastructure {
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDBSettings:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDBSettings:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Schedule> Schedules => _database.GetCollection<Schedule>("Schedules");
        public IMongoCollection<Person> Persons => _database.GetCollection<Person>("Persons");
    }
}

