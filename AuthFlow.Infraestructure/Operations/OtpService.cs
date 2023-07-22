using AuthFlow.Application.DTOs;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Application.Use_cases.Interface.Operations;
using AuthFlow.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

// The namespace for operations in the infrastructure layer
namespace AuthFlow.Infraestructure.Operations
{
    // The OtpService class handles operations related to OTP (One Time Password)
    public class OtpService : IOtpService
    {
        // Dependencies injected via constructor
        private readonly Random _random = new Random();
        private readonly IDistributedCache _distributedCache;
        private readonly IEmailService _emailService;
        private readonly ILogService _externalLogService;

        // Constructor that accepts IDistributedCache, IEmailService and ILogService as parameters
        public OtpService(IDistributedCache distributedCache, IEmailService emailService, ILogService externalLogService)
        {
            _distributedCache = distributedCache;
            _emailService = emailService;
            _externalLogService = externalLogService;
        }

        // Generate a one time password (OTP) and send it via email
        public async Task<OperationResult<bool>> GenerateOtp(string email)
        {
            try
            {
                // Generate an OTP
                var otp = new string(Enumerable.Range(0, 6).Select(x => (char)('0' + _random.Next(0, 10))).ToArray());
                var byteArray = _distributedCache.Get(email);
                if (byteArray != null)
                {
                    // Retrieve the OTP from the cache if it exists
                    otp = Encoding.UTF8.GetString(byteArray);
                }
                else
                {
                    // Otherwise, store the OTP in the cache for 5 minutes
                    var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
                    distributedCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(5);
                    _distributedCache.SetString(email, otp, distributedCacheEntryOptions);
                }

                // Send the OTP via email
                var result = await _emailService.SendEmailAsync(email, "Your OTP", $"Your OTP is: {otp}");
                if (!result.IsSuccessful)
                {
                    // If the email sending fails, return a failure result
                    return OperationResult<bool>.Failure(result.Message, ErrorTypes.BusinessValidationError);
                }

                // Return a success result if the OTP is generated and sent successfully
                return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                // Log the error and return a failure result if there's an exception
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedOtpService, ErrorTypes.DatabaseError);
            }
        }

        // Validates the given OTP against the one stored in cache for the given email
        public async Task<OperationResult<bool>> ValidateOtp(string email, string otp)
        {
            try
            {
                if (string.IsNullOrEmpty(otp))
                {
                    // If no OTP is submitted, return a failure result
                    return OperationResult<bool>.Success(false, Resource.OtpFailedDoesNotSubmitted);
                }

                var byteArray = _distributedCache.Get(email);
                if (byteArray == null)
                {
                    // If no OTP is found in the cache for the given email, return a failure result
                    return OperationResult<bool>.Success(false, Resource.OtpFailedDoesNotExist);
                }

                // Retrieve the OTP from the cache
                string storeOtp = Encoding.UTF8.GetString(byteArray);
                if (string.IsNullOrEmpty(storeOtp))
                {
                    // If no OTP is found in the cache for the given email, return a failure result
                    return OperationResult<bool>.Success(false, Resource.OtpFailedDoesNotExist);
                }

                if (!storeOtp.Equals(otp))
                {
                    // If the submitted OTP does not match the stored OTP, return a failure result
                    return OperationResult<bool>.Success(false, Resource.OtpFailedDoesNotEquals);
                }

                // Return a success result if the OTP is validated successfully
                return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                // Log the error and return a failure result if there's an exception
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedOtpService, ErrorTypes.DatabaseError);
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
