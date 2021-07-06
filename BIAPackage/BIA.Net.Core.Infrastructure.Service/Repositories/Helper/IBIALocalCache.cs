// <copyright file="BIALocalCache.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    public interface IBIALocalCache
    {
        Task Add(string key, object item, double cacheDurationInMinute);
        Task<object> Get(string key);
        Task Remove(string key);
    }
}