// Define a namespace for use cases related to wrappers in the AuthFlow application.
namespace AuthFlow.Application.Uses_cases.Interface.Wrapper
{
    using System.Net.Http.Headers;
    using System.Net.Mail;

    /// <summary>
    /// IWrapper provides an interface for common operations such as reading HTTP content, 
    /// making HTTP POST requests, and sending emails.
    /// </summary>
    public interface IWrapper
    {
        /// <summary>
        /// Reads the content of an HTTP response as a string asynchronously.
        /// </summary>
        /// <param name="content">The content</param>
        /// <returns>The result of the operation</returns>
        Task<string> ReadAsStringAsync(HttpContent content);

        /// <summary>
        /// Sends an HTTP POST request to the specified URL with optional content and authentication headers asynchronously.
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="url">The url</param>
        /// <param name="content">The content</param>
        /// <param name="authenticationHeaderValue">The autentication header value</param>
        /// <returns>The result of the operation.</returns>
        Task<HttpResponseMessage> PostAsync(HttpClient client, string url, HttpContent? content, AuthenticationHeaderValue? authenticationHeaderValue);

        /// <summary>
        ///   Sends an email using the provided SMTP client and mail message asynchronously.
        /// </summary>
        /// <param name="smtpClient">The smtp client.</param>
        /// <param name="message">The message.</param>
        /// <returns>The result of the operation.</returns>
        Task SendMailAsync(SmtpClient smtpClient, MailMessage message);
    }
}
