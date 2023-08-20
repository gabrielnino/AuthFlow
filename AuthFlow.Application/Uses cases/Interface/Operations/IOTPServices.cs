namespace AuthFlow.Application.Use_cases.Interface.Operations
{
    /// <summary>
    /// The AuthFlow.Application.Use_cases.Interface.Operations namespace contains interface definitions for various operations in the application.
    /// These interfaces define contracts for services provided by the application, abstracting the underlying implementation details.
    /// </summary>
    using AuthFlow.Application.DTOs;

    /// <summary>
    /// Defines the contract for an OTP (One-Time Password) service.
    /// This service is responsible for generating and validating OTPs, which are usually used for verification processes in the application.
    /// Different implementations of this interface can be swapped in and out as needed, allowing for flexibility and maintainability.
    /// </summary>
    public interface IOTPServices
    {
        /// <summary>
        /// Generates an OTP for a specific email.
        /// </summary>
        /// <param name="email">The email for which the OTP should be generated.</param>
        /// <returns>
        /// An <see cref="OperationResult{T}"/> which encapsulates the outcome of the OTP generation.
        /// The result is a boolean indicating whether the OTP was successfully generated or not.
        /// The implementation of this method should handle the actual process of generating and storing the OTP, potentially sending it to the email provided.
        /// </returns>
        Task<OperationResult<bool>> GenerateOtp(string email);

        /// <summary>
        /// Validates the provided OTP against the one stored for the given email.
        /// </summary>
        /// <param name="email">The email associated with the OTP.</param>
        /// <param name="otp">The OTP to validate.</param>
        /// <returns>
        /// An <see cref="OperationResult{T}"/> which encapsulates the outcome of the OTP validation.
        /// The result is a boolean indicating whether the OTP validation was successful or not.
        /// The implementation of this method should handle the actual process of OTP validation, checking the provided OTP against the one stored.
        /// </returns>
        Task<OperationResult<bool>> ValidateOtp(string email, string otp);
    }
}
