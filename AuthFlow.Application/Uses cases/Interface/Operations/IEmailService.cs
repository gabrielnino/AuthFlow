using AuthFlow.Application.DTOs;

namespace AuthFlow.Application.Uses_cases.Interface.Operations
{
    public interface IEmailService
    {
        Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message);
    }
}
