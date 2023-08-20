namespace AuthFlow.Application.Interfaces
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Domain.DTO;

    /// <summary>
    /// The IReCaptchaService interface defines the contract for a reCAPTCHA validation service.
    /// </summary>
    public interface IReCaptchaService
    {
        /// <summary>
        /// The Validate method takes a reCAPTCHA token as input and returns an OperationResult which encapsulates 
        /// </summary>
        /// <param name="token">The token</param>
        /// <returns>The result of operation</returns>
        Task<OperationResult<bool>> Validate(ReCaptcha token);
    }
}
