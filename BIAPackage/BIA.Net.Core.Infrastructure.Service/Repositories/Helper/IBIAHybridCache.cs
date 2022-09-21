using System.Threading.Tasks;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    public interface IBIAHybridCache
    {
        BIAHybridCache.CacheMode DefaultCacheMode { get; set; }

        Task AddDefaultMode(string key, object item, double cacheDurationInMinute);
        Task AddDistributed(string sid, object item, double cacheDurationInMinute);
        Task AddLocal(string key, object item, double cacheDurationInMinute);
        Task<T> GetAllSources<T>(string key);
        Task<T> GetDefaultMode<T>(string key);
        Task<T> GetLocal<T>(string key);
        Task<T> GetDistibuted<T>(string key);
        Task RemoveAllSources(string key);
        Task RemoveDefaultMode(string key);
        Task RemoveDistributed(string key);
        Task RemoveLocal(string key);
    }
}