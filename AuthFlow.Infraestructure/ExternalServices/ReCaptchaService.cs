using AuthFlow.Application.DTOs;
using AuthFlow.Application.Interfaces;
using AuthFlow.Application.Uses_cases.Interface.ExternalServices;
using AuthFlow.Domain.DTO;
using AuthFlow.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AuthFlow.Infraestructure.ExternalServices
{
    public class ReCaptchaService : IReCaptchaService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        protected readonly ILogService _externalLogService;

        public ReCaptchaService(IConfiguration configuration, HttpClient httpClient, ILogService externalLogService)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _externalLogService = externalLogService;
        }

        public async Task<OperationResult<bool>> Validate(ReCaptcha token)
        {
            try
            {
                var secret = _configuration.GetSection("reCAPTCHA:SecretKey").Value;
                var url = _configuration.GetSection("reCAPTCHA:Url").Value;

                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("secret", secret),
                    new KeyValuePair<string, string>("respuesta", token?.Token)//,
                    //new KeyValuePair<string, string>("remoteip", "127.0.0.1")
                };

                var content = new FormUrlEncodedContent(values);

                var response = await _httpClient.PostAsync(url, content);
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonData = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonString);
                return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
                /*
                if (jsonData.Success)
                {
                    return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
                }

                return OperationResult<bool>.Success(false, Resource.GlobalOkMessage);
                */
            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedRecaptchaService);
            }
        }

        protected static Log GetLogError(Exception ex, object entity, OperationExecute operation)
        {
            var message = $"Error Message: {ex.Message}  StackTrace: {ex.StackTrace}";
            var log = Log.Error(message, entity, operation);
            return log;
        }
    }
}
