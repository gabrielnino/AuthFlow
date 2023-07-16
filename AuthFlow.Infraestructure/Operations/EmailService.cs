using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Application.Use_cases.Interface.Operations;

// The namespace for operations in the infrastructure layer
namespace AuthFlow.Infraestructure.Operations
{
    // The EmailService class handles the sending of emails
    public class EmailService : IEmailService
    {
        // Dependencies injected via constructor
        private readonly IConfiguration _configuration;
        private readonly ILogService _externalLogService;

        // Constructor that accepts IConfiguration and ILogService as parameters
        public EmailService(IConfiguration configuration, ILogService externalLogService)
        {
            _configuration = configuration;
            _externalLogService = externalLogService;
        }

        // Asynchronously sends an email
        public async Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                // Get the email settings from the configuration
                var smtp = _configuration.GetSection("email:smtp").Value;
                var mailAddress = _configuration.GetSection("email:mailAddress").Value;
                var username = _configuration.GetSection("email:username").Value;
                var password = _configuration.GetSection("email:password").Value;
                var port = _configuration.GetSection("email:port").Value;

                // Prepare the mail message
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(mailAddress);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.To.Add(email);

                    // Send the email via SMTP
                    using (var client = new SmtpClient(smtp, int.Parse(port)))
                    {
                        client.Credentials = new NetworkCredential(username, password);
                        client.EnableSsl = true;
                        await client.SendMailAsync(mailMessage);
                    };
                };
                // Return the result of the operation
                return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                // Log the error and return a failure result if there's an exception
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedEmailService);
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
