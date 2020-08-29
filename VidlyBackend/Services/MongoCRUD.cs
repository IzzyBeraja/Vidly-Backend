using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidlyBackend.Models;

namespace VidlyBackend.Services
{
    public class MongoCRUD<T> : IMongoCRUD<T>
    {
        private readonly IMongoDatabase _db;

        public MongoCRUD(IVidlyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _db = client.GetDatabase(settings.DatabaseName);
        }

        public List<T> Get(string collectionName)
        {
            var collection = _db.GetCollection<T>(collectionName);
            return collection.Find(new BsonDocument()).ToList();
        }

        public List<T> Get(string collectionName, ObjectId id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            return collection.Find(filter).ToList();
        }

        public T Create(string collectionName, T record)
        {
            var collection = _db.GetCollection<T>(collectionName);
            collection.InsertOne(record);
            return record;
        }

        public void Update(string collectionName, ObjectId id, T recordIn)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            collection.ReplaceOne(filter, recordIn);

        }

        public void Remove(string collectionName, ObjectId id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }
    }
}
