using DoMCLib.Classes.Module.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DoMCRemoteControl
{
    public class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient(string baseUrl)
        {
            _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<APIStatusResponse?> GetStatusAsync()
        {
            var json = await _http.GetStringAsync("api/status");
            return JsonSerializer.Deserialize<APIStatusResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true });
        }

        public async Task PostAsync(string endpoint)
        {
            var response = await _http.PostAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
        }
    }

}
