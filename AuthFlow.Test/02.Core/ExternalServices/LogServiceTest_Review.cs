using Moq;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Domain.Entities;
using Microsoft.Extensions.Configuration;
using AuthFlow.Infraestructure.ExternalServices;
using FluentAssertions;
using AuthFlow.Application.Uses_cases.Interface.Wrapper;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using AuthFlow.Infraestructure.Other;

namespace AuthFlow.Test.ExternalServices
{
    internal class LogServiceTest
    {
        private const string MessageSuccessful = "The log was create successfully.";
        private ILogService? logService;
        private Mock<IHttpClientFactory> mockHttpClientFactory;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IConfigurationSection> mockConfigSectionUsername;
        private Mock<IConfigurationSection> mockConfigSectionPassword;
        private Mock<IConfigurationSection> mockConfigSectionUrlLogservice;
        private Mock<IWrapper> mockIHttpContentWrapper;

        [SetUp]
        public void Setup()
        {
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockConfiguration = new Mock<IConfiguration>();
            mockConfigSectionUsername = new Mock<IConfigurationSection>();
            mockConfigSectionPassword = new Mock<IConfigurationSection>();
            mockConfigSectionUrlLogservice = new Mock<IConfigurationSection>();
            mockIHttpContentWrapper = new Mock<IWrapper>();
        }

        private void SetBehavior(string username = "admin", string password = "password", string urllogservice = "url")
        {
            SetConfigurationValues(username, password, urllogservice);
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
            SetBehavior();
            var myObject = new
            {
                Name = "Test Object",
                Description = "This is a description of the test object."
            };

            var ex = new Exception("This is a exception of the test log");
            var log = Util.GetLogError(ex, myObject, OperationExecute.Activate);

            // When
            var result = logService.CreateLog(log);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().Be(string.Empty);
            result.Result.Message.Should().Be(MessageSuccessful);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_CreateLog_InvalidLogObject_Then_Success()
        {
            SetBehavior();
            // Given
            object? myObject = null;
            var ex = new Exception("This is a exception of the test log");
            var log = Util.GetLogError(ex, myObject, OperationExecute.Activate);

            // When
            var result = logService.CreateLog(log);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().Be(string.Empty);
            result.Result.Message.Should().Be("The log was create successfully.");
            return Task.CompletedTask;
        }

        [Test]
        public Task When_CreateLog_InvalidLogObject_InvalidException_Then_Failed()
        {
            SetBehavior();
            // Given
            object? myObject = null;
            var ex = default(Exception);
 
            // When

            // Then
            Assert.ThrowsAsync<Exception>(async () => Util.GetLogError(ex, myObject, OperationExecute.Activate));

            return Task.CompletedTask;
        }

        [Test]
        public Task When_CreateLog_InvalidConfiguracion_Then_Failed()
        {
            SetBehavior(username: string.Empty, password: string.Empty, urllogservice: string.Empty);
            // Given
            var myObject = new
            {
                Name = "Test Object",
                Description = "This is a description of the test object."
            };

            var ex = new Exception("This is a exception of the test log");
            var log = Util.GetLogError(ex, myObject, OperationExecute.Activate);

            // When
            var result = logService.CreateLog(log);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            return Task.CompletedTask;
        }

        private void SetConfigurationValues(string username, string password, string urllogservice)
        {
            mockConfigSectionUsername
                .Setup(x => x.Value)
                .Returns(username);
            mockConfigSectionPassword
                .Setup(x => x.Value)
                .Returns(password);
            mockConfigSectionUrlLogservice
                .Setup(x => x.Value)
                .Returns(urllogservice);
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
