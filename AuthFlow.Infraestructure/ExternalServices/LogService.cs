// The namespace for external services in the infrastructure layer
namespace AuthFlow.Infraestructure.ExternalServices
{
    using AuthFlow.Domain.Entities;
    using Newtonsoft.Json;
    using System.Text;
    using System.Net.Http.Headers;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using Microsoft.Extensions.Configuration;
    using AuthFlow.Application.Uses_cases.Interface.Wrapper;
    using AuthFlow.Application.DTOs;
    using AuthFlow.Infraestructure.Other;

    /// <summary>
    /// The LogService class handles the interactions with the logging service.
    /// </summary>
    public class LogService : ILogService
    {
        private readonly IConfiguration _configuration;
        private readonly string _username;
        private readonly string _password;
        private readonly string _urlLogservice;
        private readonly IWrapper _httpContentWrapper;
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="clientFactory">Factory for creating instances of <see cref="HttpClient"/>.</param>
        /// <param name="configuration">Application's configuration interface.</param>
        /// <param name="httpContentWrapper">Wrapper for handling HTTP content.</param>
        public LogService(IHttpClientFactory clientFactory, IConfiguration configuration, IWrapper httpContentWrapper)
        {
            _client = clientFactory.CreateClient();
            _configuration = configuration;
            _username = _configuration.GetSection("mongodb:username").Value ?? string.Empty;
            _password = _configuration.GetSection("mongodb:password").Value ?? string.Empty;
            _urlLogservice = _configuration.GetSection("logService:urlLogservice").Value ?? string.Empty;
            _httpContentWrapper = httpContentWrapper;
        }

        /// <summary>
        /// Asynchronously creates a log entry in the external log service.
        /// </summary>
        /// <param name="log">The log entity to create.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the operation result with an optional message.</returns>
        public async Task<OperationResult<string>> CreateLog(Log log)
        {
            try
            {
                var result = SetLog(log);
                if (!result.Result.IsSuccessful)
                {
                    return result.Result;
                }

                return OperationResult<string>.Success(string.Empty, Resource.SuccessfullyLogCreate);
            }
            catch (Exception ex)
            {
                var message = string.Format(Resource.FailedGolbalException, ex.Message, ex.StackTrace);
                return OperationResult<string>.FailureUnexpectedError(Resource.SuccessfullyLogCreate);
            }
        }

        /// <summary>
        /// Asynchronously gets a token for authentication with the logging service.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains the operation result with the authentication token or an error message.</returns>
        private async Task<OperationResult<string>> GetToken()
        {
            if (string.IsNullOrWhiteSpace(_urlLogservice) || string.IsNullOrWhiteSpace(_username) || string.IsNullOrWhiteSpace(_password))
            {
                return OperationResult<string>.FailureConfigurationMissingError(Resource.FailureConfigurationMissingError);
            }

            var url = $"{_urlLogservice}/Login?user={_username}&password={_password}";
            var response = await _httpContentWrapper.PostAsync(_client, url, null, null);

            if (!response.IsSuccessStatusCode)
            {
                return OperationResult<string>.FailureExtenalService(Resource.FailedGetToken);
            }

            var result = await _httpContentWrapper.ReadAsStringAsync(response.Content);
            var _tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);

            return OperationResult<string>.Success(_tokenResponse.Token, Resource.SuccessfullyGetToken);
        }

        /// <summary>
        /// Asynchronously creates a log entry in the external log service.
        /// </summary>
        /// <param name="log">The log entity to create.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the operation result with an optional message.</returns>
        private async Task<OperationResult<string>> SetLog(Log log)
        {
            var bearerToken = await GetToken();

            if (!bearerToken.IsSuccessful)
            {
                return bearerToken;
            }

            var url = $"{_urlLogservice}";
            var json = JsonConvert.SerializeObject(log);
            var content = new StringContent(json ?? "NOT_POSIBLE_GET_A_VALID_LOG", Encoding.UTF8, "application/json");
            var authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Data);

            var response = await _httpContentWrapper.PostAsync(_client, url, content, authorization);

            if (!response.IsSuccessStatusCode)
            {
                return OperationResult<string>.FailureExtenalService(Resource.FailedSetLog);
            }

            return OperationResult<string>.Success(string.Empty, Resource.SuccessfullySetLog);
        }
    }
}
