using AuthFlow.Application.DTOs;
using AuthFlow.Application.Interfaces;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Domain.DTO;
using AuthFlow.Domain.Entities;
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

        // Constructor that accepts IConfiguration, HttpClient, and ILogService as parameters
        public ReCaptchaService(IConfiguration configuration, HttpClient httpClient, ILogService externalLogService)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _externalLogService = externalLogService;
        }

        // Asynchronously validates a ReCaptcha token
        public async Task<OperationResult<bool>> Validate(ReCaptcha token)
        {
            try
            {
                // Get the secret key and url from the configuration
                var secret = _configuration.GetSection("reCAPTCHA:SecretKey").Value;
                var url = _configuration.GetSection("reCAPTCHA:Url").Value;

                // Prepare the values for the POST request to the ReCaptcha service
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("secret", secret),
                    new KeyValuePair<string, string>("respuesta", token?.Token)
                };

                // Create the content for the POST request
                var content = new FormUrlEncodedContent(values);

                // Send a POST request to the ReCaptcha service and await the response
                var response = await _httpClient.PostAsync(url, content);
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialize the response content
                var jsonData = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonString);

                // Return the result of the validation
                return OperationResult<bool>.Success(true, Resource.SuccessfullyRecaptcha);
            }
            catch (Exception ex)
            {
                // Log the error and return a failure result if there's an exception
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.FailureExtenalService(Resource.FailedRecaptchaService);
            }
        }

        // Creates a log entry for an exception
        protected static Log GetLogError(Exception ex, object entity, OperationExecute operation)
        {
            // Prepare the message for the log entry
            var message = $"Error Message: {ex.Message}  StackTrace: {ex.StackTrace}";

            // Create the log entry
            var log = Log.Error(message, entity, operation);

            // Return the log entry
            return log;
        }
    }
}
