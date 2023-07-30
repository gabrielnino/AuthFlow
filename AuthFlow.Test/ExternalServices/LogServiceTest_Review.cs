using Moq;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Domain.Entities;
using Microsoft.Extensions.Configuration;
using AuthFlow.Infraestructure.ExternalServices;
using FluentAssertions;
using AuthFlow.Application.Uses_cases.Interface.Wrapper;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace AuthFlow.Test.ExternalServices
{
    internal class LogServiceTest
    {
        //public async Task CreateLog(Log log)


        private ILogService logService;
        private Mock<IHttpClientFactory> mockHttpClientFactory;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IConfigurationSection> mockConfigSectionUsername;
        private Mock<IConfigurationSection> mockConfigSectionPassword;
        private Mock<IConfigurationSection> mockConfigSectionUrlLogservice;
        private Mock<IHttpContentWrapper> mockIHttpContentWrapper;

        [SetUp]
        public void Setup()
        {
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockConfiguration = new Mock<IConfiguration>();
            mockConfigSectionUsername = new Mock<IConfigurationSection>();
            mockConfigSectionPassword = new Mock<IConfigurationSection>();
            mockConfigSectionUrlLogservice = new Mock<IConfigurationSection>();
            mockIHttpContentWrapper = new Mock<IHttpContentWrapper>();
            SetConfigurationValues();
            SetBussinesValues();
            logService = new LogService(
                mockHttpClientFactory.Object, 
                mockConfiguration.Object, 
                mockIHttpContentWrapper.Object
                );
        }


        [Test]
        public Task When_CreateLog_ValidLogObject_Then_Success()
        {
            // Given
            var myObject = new
            {
                Name = "Test Object",
                Description = "This is a description of the test object."
            };
            var log = Log.Debug("test_message", myObject, OperationExecute.Activate);

            // When
            var result = logService.CreateLog(log);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            return Task.CompletedTask;
        }

        private void SetConfigurationValues()
        {
            mockConfigSectionUsername
                .Setup(x => x.Value)
                .Returns("admin");
            mockConfigSectionPassword
                .Setup(x => x.Value)
                .Returns("password");
            mockConfigSectionPassword
                .Setup(x => x.Value)
                .Returns("password");
            mockConfigSectionUrlLogservice
                .Setup(x => x.Value)
                .Returns("url");
            mockConfiguration
                .Setup(section => section.GetSection("mongodb:username"))
                .Returns(mockConfigSectionUsername.Object);
            mockConfiguration
                .Setup(section => section.GetSection("mongodb:password"))
                .Returns(mockConfigSectionPassword.Object);
            mockConfiguration
                .Setup(section => section.GetSection("logService:urlLogservice"))
                .Returns(mockConfigSectionUrlLogservice.Object);
        }

        private static string GetToken()
        {
            var token = new TokenResponse()
            {
                Token = "xxnDBVrrFUhVvPWQasfdasfccFUasfddasQWQPh7L"
            };

            return JsonConvert.SerializeObject(token);
        }

        private void SetBussinesValues()
        {
            string tokenReponse = GetToken();
            mockIHttpContentWrapper
                .Setup(response => response.ReadAsStringAsync(It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(tokenReponse));

            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            mockIHttpContentWrapper
                .Setup(response => response.PostAsync(
                    It.IsAny<HttpClient>(), 
                    It.IsAny<string>(), 
                    It.IsAny<HttpContent>(), 
                    It.IsAny<AuthenticationHeaderValue>()))
                .Returns(Task.FromResult(httpResponseMessage));
        }
    }
}
