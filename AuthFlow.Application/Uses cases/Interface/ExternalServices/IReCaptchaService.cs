using AuthFlow.Application.DTOs;
using AuthFlow.Domain.DTO;

namespace AuthFlow.Application.Interfaces
{
    public interface IReCaptchaService
    {
        Task<OperationResult<bool>> Validate(ReCaptcha token);
    }
}
