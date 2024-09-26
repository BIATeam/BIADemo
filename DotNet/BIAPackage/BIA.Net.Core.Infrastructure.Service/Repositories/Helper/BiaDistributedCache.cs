// <copyright file="BiaDistributedCache.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Store object in distributed with the IDistributedCache service.
    /// </summary>
    public class BiaDistributedCache : IBiaDistributedCache
    {
        private readonly IDistributedCache distibutedCache;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<BiaDistributedCache> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDistributedCache"/> class.
        /// </summary>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="logger">The logger.</param>
        public BiaDistributedCache(IDistributedCache cache, ILogger<BiaDistributedCache> logger)
        {
            this.distibutedCache = cache;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task Add<T>(string key, T item, double cacheDurationInMinute)
        {
            byte[] encodedItemResolve = ObjectToByteArray(item);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheDurationInMinute));
            await this.distibutedCache.SetAsync(key, encodedItemResolve, options);
        }

        /// <inheritdoc/>
        public async Task<T> Get<T>(string key)
        {
            byte[] encodedItemResolve = await this.distibutedCache.GetAsync(key);
            if (encodedItemResolve != null && encodedItemResolve.Length > 0)
            {
                return ByteArrayToObject<T>(encodedItemResolve);
            }

            return default(T);
        }

        /// <inheritdoc/>
        public async Task Remove(string key)
        {
            try
            {
                await this.distibutedCache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                // Not in cache
                this.logger.LogError(ex, "BIADistributedCache.Remove Not in cache");
            }
        }

        /// <summary>
        /// Convert an object to a Byte Array, using Protobuf.
        /// </summary>
        private static byte[] ObjectToByteArray<T>(T obj)
        {
            if (object.Equals(obj, default(T)))
            {
                return new byte[0];
            }

            using var stream = new MemoryStream();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(stream, obj);

            return stream.ToArray();
        }

        /// <summary>
        /// Convert a byte array to an Object of T, using Protobuf.
        /// </summary>
        private static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            using var stream = new MemoryStream();

            // Ensure that our stream is at the beginning.
            stream.Write(arrBytes, 0, arrBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(stream);
        }
    }
}
