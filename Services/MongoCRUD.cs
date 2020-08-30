using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using VidlyBackend.Profiles;

namespace VidlyBackend.Services
{
    public class MongoCRUD<T> : IDatabaseContext<T>
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

        public T Get(string collectionName, string id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            if (!ObjectId.TryParse(id, out ObjectId _id))
                return default;
            var filter = Builders<T>.Filter.Eq("_id", _id);
            return collection.Find(filter).FirstOrDefault();
        }

        public T Create(string collectionName, T record)
        {
            var collection = _db.GetCollection<T>(collectionName);
            collection.InsertOne(record);
            return record;
        }

        // No Upsert. Does nothing when id is wrong length
        public void Update(string collectionName, string id, T recordIn)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.ReplaceOne(filter, recordIn);

        }

        public void Remove(string collectionName, string id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.DeleteOne(filter);
        }
    }
}
