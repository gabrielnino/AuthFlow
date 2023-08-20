/// <summary>
/// The namespace for operations in the infrastructure layer.
/// </summary>
namespace AuthFlow.Infraestructure.Operations
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using AuthFlow.Application.Use_cases.Interface.Operations;
    using AuthFlow.Domain.Entities;
    using AuthFlow.Infraestructure.Other;
    using Microsoft.Extensions.Caching.Distributed;
    using System.Text;

    /// <summary>
    /// Provides functionality to handle operations related to OTP (One Time Password).
    /// </summary>
    public class OtpService : IOTPServices
    {
        // Dependencies injected via constructor
        private readonly Random _random = new Random();
        private readonly IDistributedCache _distributedCache;
        private readonly IEmailService _emailService;
        private readonly ILogService _externalLogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OtpService"/> class with specified dependencies.
        /// </summary>
        /// <param name="distributedCache">Service for distributed cache operations.</param>
        /// <param name="emailService">Service for sending emails.</param>
        /// <param name="logService">Service for logging operations.</param>
        public OtpService(IDistributedCache distributedCache, IEmailService emailService, ILogService logService)
        {
            _distributedCache = distributedCache;
            _emailService = emailService;
            _externalLogService = logService;
        }

        /// <summary>
        /// Asynchronously generates an OTP and sends it via email.
        /// </summary>
        /// <param name="email">Recipient's email address.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the operation result indicating success or failure of OTP generation.</returns>
        public async Task<OperationResult<bool>> GenerateOtp(string email)
        {
            try
            {
                // Generate an OTP
                var otp = new string(Enumerable.Range(0, 6).Select(x => (char)('0' + _random.Next(0, 10))).ToArray());
                var byteArray = _distributedCache.Get(email);
                var empty = new byte[0];

                if (byteArray != null && byteArray.Length > 0)
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
                    return OperationResult<bool>.FailureBusinessValidation(result.Message);
                }

                // Return a success result if the OTP is generated and sent successfully
                return OperationResult<bool>.Success(true, Resource.SuccessfullyOTPGenerate);
            }
            catch (Exception ex)
            {
                // Log the error and return a failure result if there's an exception
                var log = Util.GetLogError(ex, email, OperationExecute.GenerateOtp);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        /// <summary>
        /// Validates the provided OTP against the stored OTP in cache for a given email.
        /// </summary>
        /// <param name="email">The email for which the OTP was generated.</param>
        /// <param name="otp">The OTP to validate.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the operation result indicating success or failure of OTP validation.</returns>
        public async Task<OperationResult<bool>> ValidateOtp(string email, string otp)
        {
            try
            {
                if (!HasStrig(otp))
                {
                    // If no OTP is submitted, return a failure result
                    return OperationResult<bool>.FailureBusinessValidation(Resource.OtpFailedDoesNotSubmitted);
                }

                var byteArray = _distributedCache.Get(email);
                if (byteArray == null)
                {
                    // If no OTP is found in the cache for the given email, return a failure result
                    return OperationResult<bool>.FailureBusinessValidation(Resource.OtpFailedDoesNotExist);
                }

                // Retrieve the OTP from the cache
                string storeOtp = Encoding.UTF8.GetString(byteArray);
                if (!HasStrig(storeOtp))
                {
                    // If no OTP is found in the cache for the given email, return a failure result
                    return OperationResult<bool>.FailureBusinessValidation(Resource.OtpFailedDoesNotExist);
                }

                if (!storeOtp.Equals(otp))
                {
                    // If the submitted OTP does not match the stored OTP, return a failure result
                    return OperationResult<bool>.FailureBusinessValidation(Resource.OtpFailedDoesNotEquals);
                }

                // Return a success result if the OTP is validated successfully
                return OperationResult<bool>.Success(true, Resource.SuccessfullyOTPValidate);
            }
            catch (Exception ex)
            {
                var otpClass = new
                {
                    Email = email,
                    Otp = otp
                };

                // Log the error and return a failure result if there's an exception
                var log = Util.GetLogError(ex, otpClass, OperationExecute.ValidateOtp);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.FailureDatabase(Resource.FailedRecaptchaService);
            }
        }


        /// <summary>
        /// Validates if the provided string is neither null nor whitespace.
        /// </summary>
        /// <param name="str">String to validate.</param>
        /// <returns>Boolean indicating whether the string has content or not.</returns>
        private bool HasStrig(string str)
        {
            return !(string.IsNullOrWhiteSpace(str) || string.IsNullOrEmpty(str));
        }
    }
}
