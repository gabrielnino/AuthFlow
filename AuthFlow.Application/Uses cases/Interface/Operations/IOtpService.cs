using AuthFlow.Application.DTOs;

namespace AuthFlow.Application.Uses_cases.Interface.Operations
{
    public interface IOtpService
    {
        Task<OperationResult<bool>> GenerateOtp(string email);
        Task<OperationResult<bool>> ValidateOtp(string email, string otp);
    }
}
