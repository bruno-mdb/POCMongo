using System;
using System.IO;
using System.Linq;
using Demo.Models.MongoDb;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Demo.Context
{
    public class MongoDbContext : BaseMongoDbContext
    {
        public IMongoCollection<TipoPedido> TipoPedidos { get; }

        public MongoDbContext()
        {
            TipoPedidos = Database.GetCollection<TipoPedido>("TipoPedidos");
        }
    }

    public abstract class BaseMongoDbContext
    {
        public BaseMongoDbContext()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
            //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            Database = client.GetDatabase("POC");

            ConventionRegistry.Register("camel case", new ConventionPack
            {
                new CamelCaseElementNameConvention()
            }, t => true);
        }

        protected IMongoDatabase Database { get; }

        public IMongoCollection<TModel> Set<TModel>()
        {
            var type = typeof(IMongoCollection<TModel>);
            var instance = this.GetType().GetProperties()
              .SingleOrDefault(w => w.GetValue(this).GetType().GetInterfaces().Any(a => a.FullName == type.FullName))
              .GetValue(this);

            return (IMongoCollection<TModel>)instance;
        }
    }
}
