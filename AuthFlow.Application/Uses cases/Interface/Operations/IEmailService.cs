namespace AuthFlow.Application.Use_cases.Interface.Operations
{
    //The AuthFlow.Application.Use_cases.Interface.Operations namespace contains interface definitions for various operations in the application.
    using AuthFlow.Application.DTOs;


    /// <summary>
    /// The IEmailService interface defines the contract for an email sending service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// The SendEmailAsync method is responsible for asynchronously sending an email.
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="subject">The subject</param>
        /// <param name="message">The message</param>
        /// <returns>The result of operation</returns>
        Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message);
    }
}
