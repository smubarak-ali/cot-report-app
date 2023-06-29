using System;
using COTReport.DAL.Entity;
using COTReport.Service.Interface;
using Microsoft.Extensions.Caching.Distributed;

namespace COTReport.Service.Implementation
{
    public class SentimentCacheService : CacheService<List<Sentiment?>>, ICacheService<List<Sentiment?>>
    {
        public SentimentCacheService(IDistributedCache cache) : base(cache)
        {
        }
    }
}

