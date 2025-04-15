using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using KrisInfoBlazor.ExternalAPI.Models;




namespace KrisInfoBlazor.ExternalAPI.Services
{
    public class KrisInfoService
    {
        private readonly HttpClient _http;

        public KrisInfoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<NewsItem>> GetNewsAsync()
        {
            var response = await _http.GetStringAsync("https://api.krisinformation.se/v3/news");
            var items = JsonConvert.DeserializeObject<List<NewsItem>>(response);
            return items ?? new List<NewsItem>();
        }
    }
}
