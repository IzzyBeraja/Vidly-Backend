using System.Collections.Generic;

namespace VidlyBackend.Services
{
    public interface IDatabaseContext<T>
    {
        T Create(string collectionName, T record);
        List<T> Get(string collectionName);
        T Get(string collectionName, string id);
        void Remove(string collectionName, string id);
        void Update(string collectionName, string id, T recordIn);
    }
}