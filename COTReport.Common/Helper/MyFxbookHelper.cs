using System.Net;
using COTReport.Common.Exceptions;
using COTReport.Common.Model;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace COTReport.Common.Helper
{
    public class MyFxbookHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        private readonly string MyFxbook_Session = "MyFxbook_Session";
        private readonly string MyFxbook_Sentiments = "MyFxbook_Sentiments";

        public MyFxbookHelper(IMemoryCache cache)
        {
            _cache = cache;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MYFXBOOK_URL"));
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public async Task<MyFxbookmodel> GetSeniments()
        {
            if (!_cache.TryGetValue(MyFxbook_Sentiments, out string sentiments))
            {
                string url = $"api/get-community-outlook.json?session={await MyFxbookLogin()}";
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new ExternalApiException("Failed the request when calling the myfxbook sentiments api");

                var responseStr = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<MyFxbookmodel>(responseStr);
                if (responseModel.Symbols != null && responseModel.Symbols.Count > 0)
                    _cache.Set(MyFxbook_Sentiments, JsonConvert.SerializeObject(responseModel), absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(30));

                return responseModel;
            }

            var data = JsonConvert.DeserializeObject<MyFxbookmodel>(sentiments);
            return data;
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
                if (!string.IsNullOrEmpty(responseModel.Session))
                    _cache.Set(MyFxbook_Session, responseModel.Session, absoluteExpirationRelativeToNow: TimeSpan.FromDays(1));

                return responseModel.Session;
            }

            return session;
        }
    }
}