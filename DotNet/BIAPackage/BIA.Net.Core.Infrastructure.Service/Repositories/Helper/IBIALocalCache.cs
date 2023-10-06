// <copyright file="IBiaLocalCache.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using System.Threading.Tasks;

    public interface IBiaLocalCache
    {
        Task Add<T>(string key, T item, double cacheDurationInMinute);

        Task<T> Get<T>(string key);

        Task Remove(string key);
    }
}