using System.Threading.Tasks;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    public interface IBIADistributedCache
    {
        Task Add<T>(string key, T item, double cacheDurationInMinute);
        Task<T> Get<T>(string key);
        Task Remove(string key);
    }
}