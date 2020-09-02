using System.Collections.Generic;

namespace VidlyBackend.Services
{
    public interface IDatabaseContext
    {
        T Create<T>(string collectionName, T record);
        List<T> Get<T>(string collectionName);
        T Get<T>(string collectionName, string id);
        T Get<T>(string collectionName, string fieldName, string searchValue);
        bool Get<T>(string collectionName, string id, out T record);
        void Remove<T>(string collectionName, string id);
        void Update<T>(string collectionName, string id, T recordIn);
    }
}