using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Application.Use_cases.Interface.Operations;
using AuthFlow.Infraestructure.ExternalServices;
using AuthFlow.Infraestructure.Other;
using AuthFlow.Application.Uses_cases.Interface.Wrapper;

// The namespace for operations in the infrastructure layer
namespace AuthFlow.Infraestructure.Operations
{
    // The EmailService class handles the sending of emails
    public class EmailService : IEmailService
    {
        // Dependencies injected via constructor
        private readonly IConfiguration _configuration;
        private readonly ILogService _externalLogService;
        private readonly IWrapper _httpContentWrapper;

        // Constructor that accepts IConfiguration and ILogService as parameters
        public EmailService(IConfiguration configuration, ILogService externalLogService, IWrapper httpContentWrapper)
        {
            _configuration = configuration;
            _externalLogService = externalLogService;
            _httpContentWrapper = httpContentWrapper;
        }

        // Asynchronously sends an email
        public async Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                // Get the email settings from the configuration
                var smtp = _configuration.GetSection("email:smtp").Value ?? string.Empty;
                var mailAddress = _configuration.GetSection("email:mailAddress").Value ?? string.Empty;
                var username = _configuration.GetSection("email:username").Value ?? string.Empty;
                var password = _configuration.GetSection("email:password").Value ?? string.Empty;
                var port = _configuration.GetSection("email:port").Value ?? string.Empty;

                if (validateConfiguration(smtp, mailAddress, username, password, port))
                {
                    return OperationResult<bool>.FailureConfigurationMissingError(Resource.FailureConfigurationMissingErrorSendEmail);
                }

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
                        await _httpContentWrapper.SendMailAsync(client, mailMessage);
                    };
                };
                // Return the result of the operation
                return OperationResult<bool>.Success(true, Resource.SuccessfullyEmail);
            }
            catch (Exception ex)
            {
                // Log the error and return a failure result if there's an exception
                var log = Util.GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.FailureDatabase(Resource.FailedEmailService);
            }
        }

        private static bool validateConfiguration(string smtp, string mailAddress, string username, string password, string port)
        {
            return 
                !Util.HasString(smtp) || 
                !Util.HasString(mailAddress) || 
                !Util.HasString(username) || 
                !Util.HasString(password) || 
                !Util.HasString(port);
        }
    }
}
