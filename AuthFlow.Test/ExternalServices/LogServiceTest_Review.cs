using AuthFlow.Application.DTOs;
using AuthFlow.Application.Use_cases.Interface.Operations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Domain.Entities;
using Microsoft.Extensions.Configuration;
using AuthFlow.Infraestructure.ExternalServices;
using System.Runtime.InteropServices;
using FluentAssertions;
using System.Configuration;
using System.Net.Http;
using AuthFlow.Application.Uses_cases.Interface.Wrapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Headers;

namespace AuthFlow.Test.ExternalServices
{
    internal class LogServiceTest
    {
        //public async Task CreateLog(Log log)


        private ILogService _LogService;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IConfigurationSection> _mockConfigSectionUsername;
        private Mock<IConfigurationSection> _mockConfigSectionPassword;
        private Mock<IConfigurationSection> _mockConfigSectionUrlLogservice;
        private Mock<IHttpContentWrapper> _mockIHttpContentWrapper;
        private Mock<Stream> _mockStream;

        [SetUp]
        public void Setup()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfigSectionUsername = new Mock<IConfigurationSection>();
            _mockConfigSectionPassword = new Mock<IConfigurationSection>();
            _mockConfigSectionUrlLogservice = new Mock<IConfigurationSection>();
            _mockIHttpContentWrapper = new Mock<IHttpContentWrapper>();
            _mockStream = new Mock<Stream>();

        }
        

        [Test]
        public async Task When_CreateLog_ValidLogObject_Then_Success()
        {
            // Given
            var myObject = new
            {
                Name = "Test Object",
                Description = "This is a description of the test object."
            };

            SetConfigurationValues();
            var tokenReponse = @"
            {
                ""token"": ""xxnDBVrrFUhVvPWQasfdasfccFUasfddasQWQPh7L""
            }";

            _mockIHttpContentWrapper
                .Setup(response => response.ReadAsStringAsync(It.IsAny<HttpContent>()))
                .Returns(Task<string>.FromResult(tokenReponse));
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            _mockIHttpContentWrapper
            .Setup(response => response.PostAsync(It.IsAny<HttpClient>(), It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<AuthenticationHeaderValue>()))
            .Returns(Task.FromResult(httpResponseMessage));
            _LogService = new LogService(_mockHttpClientFactory.Object, _mockConfiguration.Object, _mockIHttpContentWrapper.Object);
            var log = Log.Debug("test_message", myObject, OperationExecute.Activate);
            var result = _LogService.CreateLog(log);
            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
        }

        private void SetConfigurationValues()
        {
            _mockConfigSectionUsername
                .Setup(x => x.Value)
                .Returns("admin");
            _mockConfigSectionPassword
                .Setup(x => x.Value)
                .Returns("password");
            _mockConfigSectionPassword
                .Setup(x => x.Value)
                .Returns("password");
            _mockConfigSectionUrlLogservice
                .Setup(x => x.Value)
                .Returns("url");
            _mockConfiguration
                .Setup(section => section.GetSection("mongodb:username"))
                .Returns(_mockConfigSectionUsername.Object);
            _mockConfiguration
                .Setup(section => section.GetSection("mongodb:password"))
                .Returns(_mockConfigSectionPassword.Object);
            _mockConfiguration
                .Setup(section => section.GetSection("logService:urlLogservice"))
                .Returns(_mockConfigSectionUrlLogservice.Object);
        }
    }
}
