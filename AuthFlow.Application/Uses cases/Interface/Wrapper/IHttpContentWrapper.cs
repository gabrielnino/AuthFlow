using System.Net.Http.Headers;

namespace AuthFlow.Application.Uses_cases.Interface.Wrapper
{
    public interface IHttpContentWrapper
    {
        Task<string> ReadAsStringAsync(HttpContent content);
        Task<HttpResponseMessage> PostAsync(HttpClient client, string url, HttpContent? content, AuthenticationHeaderValue? authenticationHeaderValue);
    }
}
