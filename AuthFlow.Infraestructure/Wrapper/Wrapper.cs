/// <summary>
/// Provides wrappers around HTTP and email-related operations.
/// </summary>
namespace AuthFlow.Infraestructure.Wrapper
{
    using AuthFlow.Application.Uses_cases.Interface.Wrapper;
    using System.Net.Http.Headers;
    using System.Net.Mail;

    /// <summary>
    /// Implements the <see cref="IWrapper"/> interface to provide wrappers for HTTP and email operations.
    /// </summary>
    public class Wrapper : IWrapper
    {
        /// <summary>
        /// Sends an HTTP POST request to the specified URL with the provided content and authentication headers.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> instance to use for sending the request.</param>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="content">The HTTP content to send with the request.</param>
        /// <param name="authenticationHeaderValue">Optional authentication header value to set on the request.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous send operation. The task result contains the <see cref="HttpResponseMessage"/> sent by the server.</returns>
        public Task<HttpResponseMessage> PostAsync(HttpClient client, string url, HttpContent? content, AuthenticationHeaderValue? authenticationHeaderValue)
        {
            // Add the bearer token to the request headers
            if (authenticationHeaderValue == null)
            {
                client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }

            return client.PostAsync(url, content);
        }

        /// <summary>
        /// Reads the content of an HTTP request or response as a string.
        /// </summary>
        /// <param name="content">The HTTP content to read.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous read operation. The task result contains the content read as a string.</returns>
        public Task<string> ReadAsStringAsync(HttpContent content)
        {
            return content.ReadAsStringAsync();
        }

        /// <summary>
        /// Sends an email asynchronously using the specified SMTP client and message.
        /// </summary>
        /// <param name="smtpClient">The SMTP client to use for sending the email.</param>
        /// <param name="message">The email message to send.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous email send operation.</returns>
        public Task SendMailAsync(SmtpClient smtpClient, MailMessage message)
        {
            return smtpClient.SendMailAsync(message);
        }
    }
}
