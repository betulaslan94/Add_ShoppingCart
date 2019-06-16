using Ciceksepeti.Entities;
using Ciceksepeti.Entities.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ciceksepeti.Data.DbContext
{
    public class DbContext<T> where T : class
    {
        private readonly IMongoDatabase database = null;

        public DbContext()
        {
            var configuration = new ConfigurationBuilder();
            configuration.AddJsonFile("appsettings.json");
            var client = new MongoClient(configuration.Build().GetSection("DemoDBConnectionString").Value.ToString());
            database = client?.GetDatabase(configuration.Build().GetSection("DemoDatabaseName").Value.ToString());
        }

        public IMongoCollection<T> dbModel
        {
            get
            {
                return database.GetCollection<T>(nameof(T));
            }
        }

        public IMongoCollection<Product> Product
        {
            get
            {
                return database.GetCollection<Product>(nameof(Product));
            }
        }

        public IMongoCollection<ShoppingCart> ShoppingCart
        {
            get
            {
                return database.GetCollection<ShoppingCart>(nameof(ShoppingCart));
            }
        }
    }
}
