// Define a namespace for use cases related to wrappers in the AuthFlow application.
namespace AuthFlow.Application.Uses_cases.Interface.Wrapper
{
    using System.Net.Http.Headers;
    using System.Net.Mail;

    // IWrapper provides an interface for common operations such as reading HTTP content, 
    // making HTTP POST requests, and sending emails.
    public interface IWrapper
    {
        // Reads the content of an HTTP response as a string asynchronously.
        Task<string> ReadAsStringAsync(HttpContent content);

        // Sends an HTTP POST request to the specified URL with optional content and authentication headers asynchronously.
        Task<HttpResponseMessage> PostAsync(HttpClient client, string url, HttpContent? content, AuthenticationHeaderValue? authenticationHeaderValue);

        // Sends an email using the provided SMTP client and mail message asynchronously.
        Task SendMailAsync(SmtpClient smtpClient, MailMessage message);
    }
}
