using System.Collections.Generic;

namespace VidlyBackend.Services
{
    public interface IDatabaseContext
    {
        T Create<T>(string collectionName, T record);
        List<T> Get<T>(string collectionName);
        T Get<T>(string collectionName, string id);
        void Remove<T>(string collectionName, string id);
        void Update<T>(string collectionName, string id, T recordIn);
    }
}