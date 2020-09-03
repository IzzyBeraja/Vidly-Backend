using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using VidlyBackend.Profiles;

namespace VidlyBackend.Services
{
    public class MongoCRUD : IDatabaseContext
    {
        private readonly IMongoDatabase _db;

        public MongoCRUD(IVidlyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _db = client.GetDatabase(settings.DatabaseName);
        }

        public List<T> Get<T>(string collectionName)
        {
            var collection = _db.GetCollection<T>(collectionName);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T Get<T>(string collectionName, string id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            if (!ObjectId.TryParse(id, out ObjectId _id))
                return default;
            var filter = Builders<T>.Filter.Eq("_id", _id);
            return collection.Find(filter).FirstOrDefault();
        }

        public bool Get<T>(string collectionName, string id, out T documentOut)
        {
            documentOut = Get<T>(collectionName, id);
            return documentOut != null;
        }

        public T Get<T>(string collectionName, string fieldName, string searchValue, bool caseSensitive)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Regex(fieldName, new BsonRegularExpression(searchValue, !caseSensitive ? "i" : ""));
            return collection.Find(filter).FirstOrDefault();
        }


        public T Create<T>(string collectionName, T record)
        {
            var collection = _db.GetCollection<T>(collectionName);
            collection.InsertOne(record);
            return record;
        }

        // No Upsert. Does nothing when id is wrong length
        public void Update<T>(string collectionName, string id, T recordIn)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.ReplaceOne(filter, recordIn);

        }

        public void Remove<T>(string collectionName, string id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.DeleteOne(filter);
        }
    }
}
