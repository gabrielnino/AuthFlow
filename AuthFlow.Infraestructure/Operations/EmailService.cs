using AuthFlow.Application.Uses_cases.Interface.Operations;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Uses_cases.Interface.ExternalServices;

namespace AuthFlow.Infraestructure.Operations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogService _externalLogService;
        public EmailService(IConfiguration configuration, ILogService externalLogService)
        {
            _configuration = configuration;
            _externalLogService = externalLogService;
        }

        public async Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtp = _configuration.GetSection("email:smtp").Value;
                var mailAddress = _configuration.GetSection("email:mailAddress").Value;
                var username = _configuration.GetSection("email:username").Value;
                var password = _configuration.GetSection("email:password").Value;
                var port = _configuration.GetSection("email:port").Value;

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
                        await client.SendMailAsync(mailMessage);
                    };
                };
                return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedEmailService);
            }
        }

        protected static Log GetLogError(Exception ex, object entity, OperationExecute operation)
        {
            var message = $"Error Message: {ex.Message}  StackTrace: {ex.StackTrace}";
            var log = Log.Error(message, entity, operation);
            return log;
        }
    }
}
