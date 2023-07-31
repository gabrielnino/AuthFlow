using AuthFlow.Application.Uses_cases.Interface.Wrapper;
using System.Net.Http.Headers;
using System.Net.Mail;

namespace AuthFlow.Infraestructure.Wrapper
{
    public class Wrapper : IWrapper
    {
        public Task<HttpResponseMessage> PostAsync(HttpClient client, string url, HttpContent? content, AuthenticationHeaderValue? authenticationHeaderValue)
        {
            // Add the bearer token to the request headers
            if (authenticationHeaderValue == null)
            {
                client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }

            return client.PostAsync(url, content);
        }

        public Task<string> ReadAsStringAsync(HttpContent content)
        {
            return content.ReadAsStringAsync();
        }

        public Task SendMailAsync(SmtpClient smtpClient, MailMessage message)
        {
            return smtpClient.SendMailAsync(message);
        }
    }
}
