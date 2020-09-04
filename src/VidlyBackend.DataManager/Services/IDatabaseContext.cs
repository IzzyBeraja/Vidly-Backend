using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManager.Services
{
    public interface IDatabaseContext
    {
        T Create<T>(string collectionName, T record);
        Task<T> CreateAsync<T>(string collectionName, T record);
        List<T> Get<T>(string collectionName);
        Task<List<T>> GetAsync<T>(string collectionName);
        T Get<T>(string collectionName, string id);
        Task<T> GetAsync<T>(string collectionName, string id);
        T Get<T>(string collectionName, string fieldName, string searchValue, bool caseSensitive = false);
        Task<T> GetAsync<T>(string collectionName, string fieldName, string searchValue, bool caseSensitive = false);
        bool Get<T>(string collectionName, string id, out T record);
        void Remove<T>(string collectionName, string id);
        Task RemoveAsync<T>(string collectionName, string id);
        void Update<T>(string collectionName, string id, T recordIn);
        Task UpdateAsync<T>(string collectionName, string id, T recordIn);
    }
}