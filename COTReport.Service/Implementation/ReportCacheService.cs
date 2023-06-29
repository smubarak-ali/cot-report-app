using System;
using COTReport.DAL.Entity;
using COTReport.Service.Interface;
using Microsoft.Extensions.Caching.Distributed;

namespace COTReport.Service.Implementation
{
    public class ReportCacheService : CacheService<List<Report?>>, ICacheService<List<Report?>>
    {
        public ReportCacheService(IDistributedCache cache) : base(cache)
        {
        }
    }
}

