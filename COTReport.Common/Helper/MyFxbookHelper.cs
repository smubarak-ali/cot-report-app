using COTReport.Common.Exceptions;
using COTReport.Common.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace COTReport.Common.Helper
{
    public class MyFxbookHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly string MyFxbook_Session = "MyFxbook_Session";

        public MyFxbookHelper(IDistributedCache cache, ILogger<MyFxbookHelper> logger)
        {
            _cache = cache;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MYFXBOOK_URL"));
            _logger = logger;
        }

        public async Task<MyFxbookmodel> GetSeniments()
        {
            string token = await MyFxbookLogin();
            _logger.LogInformation($"MyFxBook Session Token: {token}");
            string url = $"api/get-community-outlook.json?session={token}&debug=1";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new ExternalApiException("Failed the request when calling the myfxbook sentiments api");

            var responseStr = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<MyFxbookmodel>(responseStr);

            if (responseModel == null || string.Equals(responseModel.Error.ToLower(), "true") || responseModel.Symbols == null || responseModel.Symbols.Count <= 0)
                throw new ExternalApiException($"The response from myfxbook was either null or the following error => '{responseModel?.Message}'");

            return responseModel;

        }

        private async Task<string> MyFxbookLogin()
        {
            string sessionToken = await _cache.GetStringAsync(MyFxbook_Session);
            if (!string.IsNullOrEmpty(sessionToken))
            {
                return sessionToken;
            }

            string url = $"api/login.json?email={Environment.GetEnvironmentVariable("MYFXBOOK_USER")}&password={Environment.GetEnvironmentVariable("MYFXBOOK_PASS")}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new ExternalApiException("Failed the myfxbook login api request");

            var responseStr = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<MyFxbookmodel>(responseStr);
            if (responseModel == null || string.Equals(responseModel.Error.ToLower(), "true") || string.IsNullOrEmpty(responseModel.Session))
                throw new ExternalApiException($" The login response from myfxbook was either null or the following error => '{responseModel?.Message}'");

            var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddDays(120));
            await _cache.SetStringAsync(MyFxbook_Session, responseModel.Session, cacheEntryOptions);
            return responseModel?.Session ?? string.Empty;
        }
    }
}