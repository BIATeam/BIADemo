using System.Threading.Tasks;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    public interface IBIAHybridCache
    {
        BIAHybridCache.CacheMode DefaultCacheMode { get; set; }

        Task AddDefaultMode(string key, object item, double cacheDurationInMinute);
        Task AddDistributed(string sid, object item, double cacheDurationInMinute);
        Task AddLocal(string key, object item, double cacheDurationInMinute);
        Task<object> GetAllSources(string key);
        Task<object> GetDefaultMode(string key);
        Task<object> GetLocal(string key);
        Task<object> GetDistibuted(string key);
        Task RemoveAllSources(string key);
        Task RemoveDefaultMode(string key);
        Task RemoveDistributed(string key);
        Task RemoveLocal(string key);
    }
}