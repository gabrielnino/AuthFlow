// The AuthFlow.Application.Use_cases.Interface.Operations namespace contains interface definitions for various operations in the application.
// These interfaces define contracts for services provided by the application, abstracting the underlying implementation details.
using AuthFlow.Application.DTOs;

namespace AuthFlow.Application.Use_cases.Interface.Operations
{
    // The IOtpService interface defines the contract for an OTP (One-Time Password) service.
    // This service is responsible for generating and validating OTPs, which are usually used for verification processes in the application.
    // Different implementations of this interface can be swapped in and out as needed, allowing for flexibility and maintainability.
    public interface IOTPServices
    {
        // The GenerateOtp method is responsible for generating an OTP for a specific email.
        // It takes the email as a parameter, and returns an OperationResult which encapsulates the outcome of the OTP generation.
        // The result is a boolean indicating whether the OTP was successfully generated or not.
        // The implementation of this method should handle the actual process of generating and storing the OTP, potentially sending it to the email provided.
        Task<OperationResult<bool>> GenerateOtp(string email);

        // The ValidateOtp method is responsible for validating the OTP provided against the one stored for the given email.
        // It takes the email and the OTP as parameters, and returns an OperationResult which encapsulates the outcome of the OTP validation.
        // The result is a boolean indicating whether the OTP validation was successful or not.
        // The implementation of this method should handle the actual process of OTP validation, checking the provided OTP against the one stored.
        Task<OperationResult<bool>> ValidateOtp(string email, string otp);

    }
}
