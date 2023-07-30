using AuthFlow.Application.Uses_cases.Interface.Wrapper;
using System.Net.Http.Headers;

namespace AuthFlow.Infraestructure.Wrapper
{
    public class HttpContentWrapper : IHttpContentWrapper
    {
        public Task<HttpResponseMessage> PostAsync(HttpClient client, string url, HttpContent? content, AuthenticationHeaderValue? authenticationHeaderValue)
        {
            // Add the bearer token to the request headers
            if(authenticationHeaderValue == null)
            {
                client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }

            return client.PostAsync(url, content);
        }

        public Task<string> ReadAsStringAsync(HttpContent content)
        {
            return content.ReadAsStringAsync();
        }
    }
}
