using System;
using COTReport.Service.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace COTReport.Service.Implementation
{
	public class CacheService<T> : ICacheService<T>
	{
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
		{
            _cache = cache;
		}

        public async Task<T?> GetDataAsync(string key)
        {
            var cachedData = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedData))
                return default;

            var mappedObj = JsonConvert.DeserializeObject<T>(cachedData);
            return mappedObj;
        }

        public async Task SaveInCacheAsync(string key, T model, int absoluteExpiration = 10, int slidingExpiration = 5)
        {
            var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(absoluteExpiration)).SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration));
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(model), cacheEntryOptions);
        }
    }
}

