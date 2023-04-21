using COTReport.Common.Exceptions;
using COTReport.Common.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace COTReport.Common.Helper
{
    public class MyFxbookHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        private readonly string MyFxbook_Session = "MyFxbook_Session";
        private readonly string MyFxbook_Sentiments = "MyFxbook_Sentiments";

        public MyFxbookHelper(IMemoryCache cache, ILogger<MyFxbookHelper> logger)
        {
            _cache = cache;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MYFXBOOK_URL"));
            _logger = logger;
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public async Task<MyFxbookmodel> GetSeniments()
        {
            if (!_cache.TryGetValue(MyFxbook_Sentiments, out string sentiments))
            {
                string url = $"api/get-community-outlook.json?session={await MyFxbookLogin()}&debug=1";
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new ExternalApiException("Failed the request when calling the myfxbook sentiments api");

                var responseStr = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<MyFxbookmodel>(responseStr);

                if (responseModel == null || !string.IsNullOrEmpty(responseModel.Error))
                    throw new ExternalApiException($"The response from myfxbook was either null or the following error => '{responseModel?.Message}'");

                if (responseModel.Symbols == null || responseModel.Symbols.Count <= 0)
                    throw new ExternalApiException("Failed the get the sentiments when calling the myfxbook api");

                _cache.Set(MyFxbook_Sentiments, JsonConvert.SerializeObject(responseModel), absoluteExpirationRelativeToNow: TimeSpan.FromHours(1.5));
                return responseModel ?? new MyFxbookmodel();
            }

            var data = JsonConvert.DeserializeObject<MyFxbookmodel>(sentiments);
            return data ?? new MyFxbookmodel();
        }

        private async Task<string> MyFxbookLogin()
        {
            if (!_cache.TryGetValue(MyFxbook_Session, out string session))
            {
                string url = $"api/login.json?email={Environment.GetEnvironmentVariable("MYFXBOOK_USER")}&password={Environment.GetEnvironmentVariable("MYFXBOOK_PASS")}";
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new ExternalApiException("Failed the request when calling the myfxbook login api");

                var responseStr = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<MyFxbookmodel>(responseStr);
                if (responseModel == null)
                    throw new ExternalApiException($"The response from myfxbook was either null or the following error => '{responseModel?.Message}'");

                if (string.IsNullOrEmpty(responseModel.Session))
                    throw new ExternalApiException("Failed the get the session token when calling the myfxbook login api");

                _cache.Set(MyFxbook_Session, responseModel.Session, absoluteExpirationRelativeToNow: TimeSpan.FromDays(10));
                return responseModel?.Session ?? string.Empty;
            }

            return session;
        }
    }
}