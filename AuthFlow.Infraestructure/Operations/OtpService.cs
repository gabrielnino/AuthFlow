using AuthFlow.Application.DTOs;
using AuthFlow.Application.Uses_cases.Interface.ExternalServices;
using AuthFlow.Application.Uses_cases.Interface.Operations;
using AuthFlow.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace AuthFlow.Infraestructure.Operations
{
    public class OtpService : IOtpService
    {
        private readonly Random _random = new Random();
        private readonly IDistributedCache _distributedCache;
        private readonly IEmailService _emailService;
        private readonly ILogService _externalLogService;

        public OtpService(IDistributedCache distributedCache, IEmailService emailService, ILogService externalLogService)
        {
            _distributedCache = distributedCache;
            _emailService = emailService;
            _externalLogService = externalLogService;
        }

        public async Task<OperationResult<bool>> GenerateOtp(string email)
        {
            try
            {
                var otp = new string(Enumerable.Range(0, 6).Select(x => (char)('0' + _random.Next(0, 10))).ToArray());
                var byteArray = _distributedCache.Get(email);
                if(byteArray != null)
                {
                    otp = Encoding.UTF8.GetString(byteArray);
                }
                else
                {
                    var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
                    distributedCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(5);
                    _distributedCache.SetString(email, otp, distributedCacheEntryOptions);
                }
               
                var result = await _emailService.SendEmailAsync(email, "Your OTP", $"Your OTP is: {otp}");
                if(!result.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(result.Message);
                }

                return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedOtpService);
            }
        }

        public async Task<OperationResult<bool>> ValidateOtp(string email, string otp)
        {
            try
            {
                if (string.IsNullOrEmpty(otp))
                {
                    return OperationResult<bool>.Success(false, Resource.OtpFailedDoesNotSubmitted);
                }

                var byteArray = _distributedCache.Get(email);
                if (byteArray == null)
                {
                    return OperationResult<bool>.Success(false, Resource.OtpFailedDoesNotExist);
                }

                string storeOtp = Encoding.UTF8.GetString(byteArray);
                if (string.IsNullOrEmpty(storeOtp))
                {
                    return OperationResult<bool>.Success(false, Resource.OtpFailedDoesNotExist);
                }

                if (!storeOtp.Equals(otp))
                {
                    return OperationResult<bool>.Success(false, Resource.OtpFailedDoesNotEquals);
                }

                return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedOtpService);
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
