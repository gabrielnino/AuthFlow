namespace AuthFlow.Application.Use_cases.Interface.Operations
{
    // The AuthFlow.Application.Use_cases.Interface.Operations namespace contains interface definitions for various operations in the application.
    // These interfaces are used to define contracts for services provided by the application, and they abstract the underlying implementation details.
    using AuthFlow.Application.DTOs;


    // The IEmailService interface defines the contract for an email sending service.
    // This service is responsible for sending emails on behalf of the application.
    // By following this interface, different implementations can be swapped in and out as needed,
    // allowing for flexibility and maintainability.
    public interface IEmailService
    {
        // The SendEmailAsync method is responsible for asynchronously sending an email.
        // It accepts the recipient's email address, the subject line of the email, and the message body as parameters.
        // The method returns an OperationResult which encapsulates the outcome of the email sending operation.
        // The result is a boolean indicating whether the email was sent successfully or not.
        // The implementation of this method should handle the actual process of sending the email, 
        // potentially interacting with an external email service provider.
        Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message);
    }
}
