using AuthFlow.Domain.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using Microsoft.Extensions.Configuration;

// The namespace for external services in the infrastructure layer
namespace AuthFlow.Infraestructure.ExternalServices
{
    // The LogService class handles the interactions with the logging service
    public class LogService : ILogService
    {
        private readonly IConfiguration _configuration;
        // Constants for the user and password
        private readonly string username;
        private readonly string password;

        // HttpClient is a modern, fast and highly configurable class used for sending HTTP requests and receiving HTTP responses from a resource identified by a URI
        private readonly HttpClient _client;

        // Constructor that takes an IHttpClientFactory as a parameter
        public LogService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            // Create an HttpClient instance from the factory
            _client = clientFactory.CreateClient();
            _configuration = configuration;
            username = _configuration.GetSection("mongodb:username").Value ?? string.Empty;
            password = _configuration.GetSection("mongodb:password").Value ?? string.Empty;
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
            var url = $"https://localhost:7060/api/Log/Login?user={username}&password={password}";

            // Send a POST request to the url
            var response = await _client.PostAsync(url, null);

            // If the response indicates success, process the token
            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response
                var result = await response.Content.ReadAsStringAsync();

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
            var url = "https://localhost:7060/api/Log";

            // Serialize the log to JSON
            var json = JsonConvert.SerializeObject(log);

            // Create the content for the request
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Get the bearer token for the request
            var bearerToken = await GetToken();

            // Add the bearer token to the request headers
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            // Send a POST request to the url with the content
            var response = await _client.PostAsync(url, content);

            // If the response indicates success, return an empty string
            // If the response indicates failure, return an empty string
            if (response.IsSuccessStatusCode) ;
            return string.Empty;
        }
    }
}
