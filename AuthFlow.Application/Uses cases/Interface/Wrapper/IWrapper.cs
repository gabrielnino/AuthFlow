namespace AuthFlow.Application.Uses_cases.Interface.Wrapper
{
    using System.Net.Http.Headers;
    using System.Net.Mail;

    public interface IWrapper
    {
        Task<string> ReadAsStringAsync(HttpContent content);
        Task<HttpResponseMessage> PostAsync(HttpClient client, string url, HttpContent? content, AuthenticationHeaderValue? authenticationHeaderValue);
        Task SendMailAsync(SmtpClient smtpClient, MailMessage message);
    }
}
