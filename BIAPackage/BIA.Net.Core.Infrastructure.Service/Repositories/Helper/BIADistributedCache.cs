// <copyright file="BIADistributedCache.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;

    /// <summary>
    /// Store object in distributed with the IDistributedCache service
    /// </summary>
    public class BIADistributedCache : IBIADistributedCache
    {
        private readonly IDistributedCache distibutedCache;
        public BIADistributedCache(IDistributedCache cache)
        {
            distibutedCache = cache;
        }

        public async Task Add(string key, object item, double cacheDurationInMinute)
        {
            byte[] encodedItemResolve = ObjectToByteArray(item);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheDurationInMinute));
            await distibutedCache.SetAsync(key, encodedItemResolve, options);
        }

        public async Task<object> Get(string key)
        {
            byte[] encodedItemResolve = await distibutedCache.GetAsync(key);
            if (encodedItemResolve != null)
            {
                return ByteArrayToObject(encodedItemResolve);
            }

            return null;
        }

        public async Task Remove(string key)
        {
            try
            {
                await distibutedCache.RemoveAsync(key);
            }
            catch (Exception)
            {
                // Not in cache
            }
        }

        // Convert an object to a byte array
        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new();
            using MemoryStream ms = new();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        // Convert a byte array to an Object
        private object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new();
            BinaryFormatter binForm = new();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = binForm.Deserialize(memStream);

            return obj;
        }
    }
}
