namespace AuthFlow.Test.Infraestructure.ExternalServices
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Interfaces;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using AuthFlow.Application.Uses_cases.Interface.Wrapper;
    using AuthFlow.Domain.DTO;
    using AuthFlow.Infraestructure.ExternalServices;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Newtonsoft.Json;

    internal class ReCaptchaServiceTests
    {
        private const string MessageReCaptchaSuccessfully = "The reCAPTCHA code was validated successfully.";
        private const string MessageReCaptchaFailed = "The configuration for the ReCaptcha services is missing the secretKey or URL.";

        //private const string MessageSuccessful = "The log was create successfully.";
        private IReCaptchaService reCaptchaService;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<HttpClient> mockHttpClient;
        private Mock<ILogService> mockLogService;
        private Mock<IConfigurationSection> mockConfigSectionUrl;
        private Mock<IConfigurationSection> mockConfigSectionSecretKey;
        private Mock<IWrapper> mockIHttpContentWrapper;

        [SetUp]
        public void Setup()
        {
            mockConfiguration = new Mock<IConfiguration>();
            mockConfigSectionUrl = new Mock<IConfigurationSection>();
            mockConfigSectionSecretKey = new Mock<IConfigurationSection>();
            mockHttpClient = new Mock<HttpClient>();
            mockLogService = new Mock<ILogService>();
            mockIHttpContentWrapper = new Mock<IWrapper>();
        }

        [Test]
        public Task When_Validate_ValidTokenObject_Then_Success()
        {
            // Given
            SetBehavior();
            var token = new ReCaptcha() { Token ="asdfasdf234xaoiusoiualoi98876kjhkjh#$#$#%" };

            // When
            var result = reCaptchaService.Validate(token);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeTrue();
            result.Result.Message.Should().Be(MessageReCaptchaSuccessfully);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Validate_InvalidConfiguration_Then_Failed()
        {
            // Given
            SetBehavior(secretKey:string.Empty,url:string.Empty);
            var token = new ReCaptcha() { Token ="asdfasdf234xaoiusoiualoi98876kjhkjh#$#$#%" };

            // When
            var result = reCaptchaService.Validate(token);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be(MessageReCaptchaFailed);
            result.Result.Types.Should().Be(ErrorTypes.ConfigurationMissingError);
            return Task.CompletedTask;
        }

        private void SetBehavior(string secretKey = "secretKey", string url = "url")
        {
            SetConfigurationValues(secretKey, url);
            reCaptchaService = new ReCaptchaService(
                mockConfiguration.Object, 
                mockHttpClient.Object, 
                mockLogService.Object, 
                mockIHttpContentWrapper.Object);
        }

        private static string GetReCaptchaResponse()
        {
            var reCaptchaResponse = new ReCaptchaResponse()
            {
                ErrorCodes = new List<string>()
                {
                    "ERROR_CODE_EXAMPLE"
                },
                Success = true,
            };

            return JsonConvert.SerializeObject(reCaptchaResponse);
        }

        private void SetConfigurationValues(string secretKey, string url)
        {
            mockConfigSectionSecretKey
                .Setup(x => x.Value)
                .Returns(secretKey);
            mockConfigSectionUrl
                .Setup(x => x.Value)
                .Returns(url);
            mockConfiguration
                .Setup(section => section.GetSection("reCAPTCHA:SecretKey"))
                .Returns(mockConfigSectionSecretKey.Object);
            mockConfiguration
                .Setup(section => section.GetSection("reCAPTCHA:Url"))
                .Returns(mockConfigSectionUrl.Object);
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            mockIHttpContentWrapper
                .Setup(response => response.PostAsync(
                    It.IsAny<HttpClient>(),
                    It.IsAny<string>(),
                    It.IsAny<HttpContent>(),
                   null))
                .Returns(Task.FromResult(httpResponseMessage));

            var reCaptchaResponse = GetReCaptchaResponse();
            mockIHttpContentWrapper
                .Setup(response => response.ReadAsStringAsync(It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(reCaptchaResponse));
        }
    }
}
