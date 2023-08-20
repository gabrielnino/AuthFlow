// The namespace for operations in the infrastructure layer
namespace AuthFlow.Infraestructure.Operations
{
    using System;
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
    using System.Threading.Tasks;

    /// <summary>
    /// The EmailService class provides functionality to handle email sending operations.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogService _externalLogService;
        private readonly IWrapper _httpContentWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class with specified configurations and logging capabilities.
        /// </summary>
        /// <param name="configuration">The application's configuration interface.</param>
        /// <param name="externalLogService">Service used for logging purposes.</param>
        /// <param name="httpContentWrapper">Wrapper for handling HTTP content.</param>
        public EmailService(IConfiguration configuration, ILogService externalLogService, IWrapper httpContentWrapper)
        {
            _configuration = configuration;
            _externalLogService = externalLogService;
            _httpContentWrapper = httpContentWrapper;
        }

        /// <summary>
        /// Asynchronously sends an email to the provided recipient with the specified subject and message.
        /// </summary>
        /// <param name="email">Recipient's email address.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="message">Email body or message content.</param>
        /// <returns>A task representing the asynchronous email sending operation. The task result contains the operation result indicating success or failure.</returns>
        public async Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtp = _configuration.GetSection("email:smtp").Value ?? string.Empty;
                var mailAddress = _configuration.GetSection("email:mailAddress").Value ?? string.Empty;
                var username = _configuration.GetSection("email:username").Value ?? string.Empty;
                var password = _configuration.GetSection("email:password").Value ?? string.Empty;
                var port = _configuration.GetSection("email:port").Value ?? string.Empty;

                if (InValidateConfiguration(smtp, mailAddress, username, password, port))
                {
                    return OperationResult<bool>.FailureConfigurationMissingError(Resource.FailureConfigurationMissingErrorSendEmail);
                }

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(mailAddress);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.To.Add(email);

                    using (var client = new SmtpClient(smtp, int.Parse(port)))
                    {
                        client.Credentials = new NetworkCredential(username, password);
                        client.EnableSsl = true;
                        await _httpContentWrapper.SendMailAsync(client, mailMessage);
                    };
                };
                return OperationResult<bool>.Success(true, Resource.SuccessfullyEmail);
            }
            catch (Exception ex)
            {
                var sendEmailAsync = new
                {
                    Email = email,
                    Subject = subject,
                    Message = message,
                };
                var log = Util.GetLogError(ex, sendEmailAsync, OperationExecute.SendEmailAsync);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.FailureDatabase(Resource.FailedEmailService);
            }
        }

        /// <summary>
        /// Validates email configuration settings.
        /// </summary>
        /// <param name="smtp">SMTP server address.</param>
        /// <param name="mailAddress">Email sender's address.</param>
        /// <param name="username">SMTP server username.</param>
        /// <param name="password">SMTP server password.</param>
        /// <param name="port">SMTP server port number.</param>
        /// <returns>Boolean indicating whether the configuration is invalid.</returns>
        private static bool InValidateConfiguration(string smtp, string mailAddress, string username, string password, string port)
        {
            return
                string.IsNullOrWhiteSpace(smtp) ||
                string.IsNullOrWhiteSpace(mailAddress) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(port);
        }
    }
}
