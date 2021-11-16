using Application.Common.Models;
using Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DistributedCacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;

        public DistributedCacheProvider(IDistributedCache cache, IConfiguration config)
        {
            _cache = cache;
            _config = config;
        }
        public async Task<T> Get<T>(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    byte[] cached_data = await _cache.GetAsync(key);
                    if (cached_data != null)
                    {
                        // convert to object
                        T item = GetObject<T>(cached_data);
                        return item;
                    }
                    return default;
                }
                else
                {
                    throw new ArgumentNullException("Cache key cannot be null");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task RefreshCache(string Key)
        {
            if (!string.IsNullOrEmpty(Key))
            {
                await _cache.RefreshAsync(Key);
            }
        }

        public async Task Remove(string Key)
        {
            if (!string.IsNullOrEmpty(Key))
            {
                await _cache.RemoveAsync(Key);
            }
            else
            {
                throw new ArgumentNullException("Set cache cannot have null values");
            }


        }

        public async Task Set<T>(string Key, T entity, CacheOptions option)
        {
            try
            {
                if (entity != null && !string.IsNullOrEmpty(Key))
                {
                    byte[] data = GetBytes(entity);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(option.AbsoluteExpirySeconds ?? Convert.ToDouble(_config["CachingOptions:AbsoluteExpirySeconds"])))
                        .SetSlidingExpiration(TimeSpan.FromSeconds(option.SlidingExpirySeconds ?? Convert.ToDouble(_config["CachingOptions:SlidingExpirySeconds"])));

                    await _cache.SetAsync(Key, data, options);
                }
                else
                {
                    throw new ArgumentNullException("Set cache cannot have null values");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        private byte[] GetBytes<T>(T obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();

        }

        private T GetObject<T>(byte[] data)
        {
            if (data == null)
                return default;
            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream(data);
            object obj = bf.Deserialize(ms);
            return (T)obj;
        }
    }
}
