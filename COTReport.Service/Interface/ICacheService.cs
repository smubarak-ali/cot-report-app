using System;
namespace COTReport.Service.Interface
{
	public interface ICacheService<T>
	{
		Task<T?> GetDataAsync(string key);

		Task SaveInCacheAsync(string key, T model, int absoluteExpiration = 10, int slidingExpiration = 5);
	}
}

