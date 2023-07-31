using AuthFlow.Application.DTOs;
using AuthFlow.Application.Interfaces;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Application.Uses_cases.Interface.Wrapper;
using AuthFlow.Domain.DTO;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Other;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// The namespace for external services in the infrastructure layer
namespace AuthFlow.Infraestructure.ExternalServices
{
    // The ReCaptchaService class handles the interactions with Google's ReCaptcha service
    public class ReCaptchaService : IReCaptchaService
    {
        // Dependencies injected via constructor
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        protected readonly ILogService _externalLogService;
        protected readonly IWrapper _httpContentWrapper;

        // Constructor that accepts IConfiguration, HttpClient, and ILogService as parameters
        public ReCaptchaService(IConfiguration configuration, HttpClient httpClient, ILogService logService, IWrapper httpContentWrapper)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _externalLogService = logService;
            _httpContentWrapper = httpContentWrapper;
        }

        // Asynchronously validates a ReCaptcha token
        public async Task<OperationResult<bool>> Validate(ReCaptcha token)
        {
            try
            {
                // Get the secret key and url from the configuration
                var secret = _configuration.GetSection("reCAPTCHA:SecretKey").Value ?? string.Empty;
                var url = _configuration.GetSection("reCAPTCHA:Url").Value ?? string.Empty;

                if (!Util.HasString(secret) || !Util.HasString(url))
                {
                    return OperationResult<bool>.FailureConfigurationMissingError(Resource.FailureConfigurationMissingErrorReCaptcha);
                }

                // Prepare the values for the POST request to the ReCaptcha service
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("secret", secret),
                    new KeyValuePair<string, string>("respuesta", token?.Token)
                };

                // Create the content for the POST request
                var content = new FormUrlEncodedContent(values);

                // Send a POST request to the ReCaptcha service and await the response
                var response = await _httpContentWrapper.PostAsync(_httpClient, url, content, null);
                var jsonString = await _httpContentWrapper.ReadAsStringAsync(response.Content);

                // Deserialize the response content
                var jsonData = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonString);

                // Return the result of the validation
                return OperationResult<bool>.Success(true, Resource.SuccessfullyRecaptcha);
            }
            catch (Exception ex)
            {
                // Log the error and return a failure result if there's an exception
                var log = Util.GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.FailureExtenalService(Resource.FailedRecaptchaService);
            }
        }
    }
}
