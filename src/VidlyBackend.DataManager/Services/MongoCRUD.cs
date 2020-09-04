using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using DataManager.Profiles;
using System.Threading.Tasks;

namespace DataManager.Services
{
    public class MongoCRUD : IDatabaseContext
    {
        private readonly IMongoDatabase _db;

        public MongoCRUD(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _db = client.GetDatabase(settings.DatabaseName);
        }

        public List<T> Get<T>(string collectionName)
        {
            var collection = _db.GetCollection<T>(collectionName);
            return collection.Find(new BsonDocument()).ToList();
        }

        public async Task<List<T>> GetAsync<T>(string collectionName)
        {
            return await Task.Run(() => { return Get<T>(collectionName); });
        }

        public T Get<T>(string collectionName, string id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            if (!ObjectId.TryParse(id, out ObjectId _id))
                return default;
            var filter = Builders<T>.Filter.Eq("_id", _id);
            return collection.Find(filter).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string collectionName, string id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            if (!ObjectId.TryParse(id, out ObjectId _id))
                return default;
            var filter = Builders<T>.Filter.Eq("_id", _id);
            return await collection.Find(filter).FirstOrDefaultAsync();
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

        public async Task<T> GetAsync<T>(string collectionName, string fieldName, string searchValue, bool caseSensitive)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Regex(fieldName, new BsonRegularExpression(searchValue, !caseSensitive ? "i" : ""));
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public T Create<T>(string collectionName, T record)
        {
            var collection = _db.GetCollection<T>(collectionName);
            collection.InsertOne(record);
            return record;
        }

        public async Task<T> CreateAsync<T>(string collectionName, T record)
        {
            var collection = _db.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(record);
            return record;
        }

        // No Upsert. Does nothing when id is wrong length
        public void Update<T>(string collectionName, string id, T recordIn)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.ReplaceOne(filter, recordIn);
        }

        public async Task UpdateAsync<T>(string collectionName, string id, T recordIn)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await collection.ReplaceOneAsync(filter, recordIn);
        }

        public void Remove<T>(string collectionName, string id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.DeleteOne(filter);
        }

        public async Task RemoveAsync<T>(string collectionName, string id)
        {
            var collection = _db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await collection.DeleteOneAsync(filter);
        }
    }
}
