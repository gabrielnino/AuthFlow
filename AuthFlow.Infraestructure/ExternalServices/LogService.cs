using AuthFlow.Domain.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using Microsoft.Extensions.Configuration;
using AuthFlow.Application.Uses_cases.Interface.Wrapper;

// The namespace for external services in the infrastructure layer
namespace AuthFlow.Infraestructure.ExternalServices
{
    // The LogService class handles the interactions with the logging service
    public class LogService : ILogService
    {
        private readonly IConfiguration _configuration;
        // Constants for the user and password
        private readonly string _username;
        private readonly string _password;
        private readonly string _urlLogservice;
        private readonly IHttpContentWrapper _httpContentWrapper;

        // HttpClient is a modern, fast and highly configurable class used for sending HTTP requests and receiving HTTP responses from a resource identified by a URI
        private readonly HttpClient _client;

        // Constructor that takes an IHttpClientFactory as a parameter
        public LogService(IHttpClientFactory clientFactory, IConfiguration configuration, IHttpContentWrapper httpContentWrapper)
        {
            // Create an HttpClient instance from the factory
            _client = clientFactory.CreateClient();
            _configuration = configuration;
            _username = _configuration.GetSection("mongodb:username").Value ?? string.Empty;
            _password = _configuration.GetSection("mongodb:password").Value ?? string.Empty;
            _urlLogservice = _configuration.GetSection("logService:urlLogservice").Value ?? string.Empty;
            _httpContentWrapper = httpContentWrapper;
        }

        // Asynchronously creates a log entry in the external log service
        public async Task CreateLog(Log log)
        {
            try
            {
                // Call SetLog to create the log entry
                await SetLog(log);
            }
            catch
            {
                // Silent catch for exceptions, this may be improved by adding logging or rethrowing the exception after handling it
            }
        }

        // Asynchronously gets a token for authentication with the logging service
        private async Task<string> GetToken()
        {
            // Create the url for the token request
            var url = $"{_urlLogservice}/Login?user={_username}&password={_password}";

            // Send a POST request to the url
            var response = await _httpContentWrapper.PostAsync(_client, url, null, null);

            // If the response indicates success, process the token
            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response
                var result = await _httpContentWrapper.ReadAsStringAsync(response.Content);

                // Deserialize the token from the response content
                var _tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);

                // Return the token
                return _tokenResponse.Token;
            }

            // If the response indicates failure, return an empty string
            return string.Empty;
        }

        // Asynchronously creates a log entry in the external log service
        private async Task<string> SetLog(Log log)
        {
            // Create the url for the log entry
            var url = $"{_urlLogservice}";

            // Serialize the log to JSON
            var json = JsonConvert.SerializeObject(log);

            // Create the content for the request
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Get the bearer token for the request
            var bearerToken = await GetToken();

            // Add the bearer token to the request headers
            var authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            // Send a POST request to the url with the content
            var response = await _httpContentWrapper.PostAsync(_client, url, content, authorization);

            // If the response indicates success, return an empty string
            // If the response indicates failure, return an empty string
            if (response.IsSuccessStatusCode) ;
            return string.Empty;
        }
    }
}
