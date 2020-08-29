using MongoDB.Bson;
using System.Collections.Generic;

namespace VidlyBackend.Services
{
    public interface IMongoCRUD<T>
    {
        T Create(string collectionName, T record);
        List<T> Get(string collectionName);
        List<T> Get(string collectionName, ObjectId id);
        void Remove(string collectionName, ObjectId id);
        void Update(string collectionName, ObjectId id, T recordIn);
    }
}