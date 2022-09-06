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
    using System.Xml.Serialization;

    /// <summary>
    /// Store object in distributed with the IDistributedCache service
    /// </summary>
#pragma warning disable S101 // Types should be named in PascalCase
    public class BIADistributedCache : IBIADistributedCache
#pragma warning restore S101 // Types should be named in PascalCase
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

        public async Task<T> Get<T>(string key)
        {
            byte[] encodedItemResolve = await distibutedCache.GetAsync(key);
            if (encodedItemResolve != null)
            {
                return ByteArrayToObject<T>(encodedItemResolve);
            }

            return default(T);
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
        private byte[] ObjectToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new();
            using MemoryStream ms = new();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        // Convert a byte array to an Object
        private T ByteArrayToObject<T>(byte[] arrBytes)
        {
            MemoryStream memStream = new();
            BinaryFormatter binForm = new();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);

            return obj;
        }

        ///// <summary>
        ///// Convert an object to a Byte Array, using Protobuf.
        ///// </summary>
        //private byte[] ObjectToByteArray<T>(T obj)
        //{
        //    if (obj == null)
        //        return null;

        //    using var stream = new MemoryStream();
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //    xmlSerializer.Serialize(stream, obj);

        //    return stream.ToArray();
        //}

        ///// <summary>
        ///// Convert a byte array to an Object of T, using Protobuf.
        ///// </summary>
        //private T ByteArrayToObject<T>(byte[] arrBytes)
        //{
        //    using var stream = new MemoryStream();

        //    // Ensure that our stream is at the beginning.
        //    stream.Write(arrBytes, 0, arrBytes.Length);
        //    stream.Seek(0, SeekOrigin.Begin);
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //    return (T)xmlSerializer.Deserialize(stream);
        //}

    }
}
