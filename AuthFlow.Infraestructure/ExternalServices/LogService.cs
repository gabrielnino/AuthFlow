using AuthFlow.Application.Uses_cases.Interface;
using AuthFlow.Domain.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using AuthFlow.Application.Uses_cases.Interface.ExternalServices;

namespace AuthFlow.Infraestructure.ExternalServices
{
    public class LogService : ILogService
    {
        private const string user = "admin";
        private const string password = "xxnDBVrrFUhVvPWQPh7LuunDBVccFUhVvooQQWQPh7L";
        private readonly HttpClient _client;

        public LogService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient();
        }

        public async Task CreateLog(Log log)
        {
            try
            {
                await SetLog(log);
            }
            catch { }
        }

        private async Task<string> GetToken()
        {
            var url = $"https://localhost:7060/api/Log/Login?user={user}&password={password}";
            var response = await _client.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var _tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);
                return _tokenResponse.Token;
            }

            return string.Empty;
        }

        private async Task<string> SetLog(Log log)
        {
            var url = "https://localhost:7060/api/Log";
            var json = JsonConvert.SerializeObject(log);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var bearerToken = await GetToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            var response = await _client.PostAsync(url, content);
            if (response.IsSuccessStatusCode) ;
            return string.Empty;
        }
    }
}
