// The namespace for external services in the infrastructure layer
namespace AuthFlow.Infraestructure.ExternalServices
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Interfaces;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using AuthFlow.Application.Uses_cases.Interface.Wrapper;
    using AuthFlow.Domain.DTO;
    using AuthFlow.Domain.Entities;
    using AuthFlow.Infraestructure.Other;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// The ReCaptchaService class handles the interactions with Google's ReCaptcha service.
    /// </summary>
    public class ReCaptchaService : IReCaptchaService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        protected readonly ILogService _externalLogService;
        protected readonly IWrapper _httpContentWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReCaptchaService"/> class.
        /// </summary>
        /// <param name="configuration">Application's configuration interface.</param>
        /// <param name="httpClient">The HttpClient instance used to send HTTP requests.</param>
        /// <param name="logService">Service used to log information.</param>
        /// <param name="httpContentWrapper">Wrapper for handling HTTP content.</param>
        public ReCaptchaService(IConfiguration configuration, HttpClient httpClient, ILogService logService, IWrapper httpContentWrapper)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _externalLogService = logService;
            _httpContentWrapper = httpContentWrapper;
        }

        /// <summary>
        /// Asynchronously validates a ReCaptcha token.
        /// </summary>
        /// <param name="token">The ReCaptcha token to validate.</param>
        /// <returns>A task representing the asynchronous validation operation. The task result contains the operation result indicating success or failure.</returns>
        public async Task<OperationResult<bool>> Validate(ReCaptcha token)
        {
            try
            {
                var secret = _configuration.GetSection("reCAPTCHA:SecretKey").Value ?? string.Empty;
                var url = _configuration.GetSection("reCAPTCHA:Url").Value ?? string.Empty;

                if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(url))
                {
                    return OperationResult<bool>.FailureConfigurationMissingError(Resource.FailureConfigurationMissingErrorReCaptcha);
                }

                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("secret", secret),
                    new KeyValuePair<string, string>("respuesta", token?.Token)
                };

                var content = new FormUrlEncodedContent(values);
                var response = await _httpContentWrapper.PostAsync(_httpClient, url, content, null);
                var jsonString = await _httpContentWrapper.ReadAsStringAsync(response.Content);
                var jsonData = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonString);

                return OperationResult<bool>.Success(true, Resource.SuccessfullyRecaptcha);
            }
            catch (Exception ex)
            {
                var log = Util.GetLogError(ex, token, OperationExecute.Validate);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    return OperationResult<bool>.FailureExtenalService(Resource.FailedRecaptchaService);
                }

                return OperationResult<bool>.FailureExtenalService(Resource.FailedRecaptchaService);
            }
        }
    }
}
