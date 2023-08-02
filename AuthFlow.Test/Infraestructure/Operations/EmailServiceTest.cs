namespace AuthFlow.Test.Infraestructure.Operations
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using AuthFlow.Application.Use_cases.Interface.Operations;
    using AuthFlow.Application.Uses_cases.Interface.Wrapper;
    using AuthFlow.Infraestructure.Operations;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using System.Net.Mail;

    public class EmailServiceTests
    {
        private const string MessageSendEmailAsync = "The email was sent successfully.";
        private const string MessageConfigurationInvalid = "The configuration for the Email services is missing one of the necessary parameters.";
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IConfigurationSection> mockConfigSectionSmtp;
        private Mock<IConfigurationSection> mockConfigSectionEmailAddress;
        private Mock<IConfigurationSection> mockConfigSectionUsername;
        private Mock<IConfigurationSection> mockConfigSectionPassword;
        private Mock<IConfigurationSection> mockConfigSectionPort;
        private Mock<IWrapper> mockIHttpContentWrapper;
        private Mock<ILogService> mockLogService;
        private IEmailService? mockIEmailService;
        private static string emailUser = "emailUser@servermail.com";
        private static string subject = "subject";
        private static string message = "message";

        [SetUp]
        public void Setup()
        {
            mockConfiguration = new Mock<IConfiguration>();
            mockLogService = new Mock<ILogService>();
        }

        private void SetBehavior(
            string? smtp = null, 
            string? emailAddress = null, 
            string? port = null, 
            string? username = null, 
            string? password = null)
        {
            string smtpDefault = smtp ?? "smtp.serveremail.com";
            string emailAddressDefault = emailAddress ?? "authflow.otp@serveremail.com";
            string portDefault = port ?? "587";
            string usernameDefault = username ?? "authflow.otp@serveremail.com";
            string passwordDefault = password ?? "wufuwrvqyvggbmjf";
            mockConfiguration = new Mock<IConfiguration>();
            mockLogService = new Mock<ILogService>();
            mockConfigSectionSmtp = new Mock<IConfigurationSection>();
            mockConfigSectionEmailAddress = new Mock<IConfigurationSection>();
            mockConfigSectionUsername = new Mock<IConfigurationSection>();
            mockConfigSectionPassword = new Mock<IConfigurationSection>();
            mockConfigSectionPort = new Mock<IConfigurationSection>();
            mockIHttpContentWrapper = new Mock<IWrapper>();
            SetConfigurationValues(smtpDefault, emailAddressDefault, portDefault, usernameDefault, passwordDefault);
            mockIEmailService = new EmailService(mockConfiguration.Object, mockLogService.Object, mockIHttpContentWrapper.Object);
        }

        [Test]
        public Task When_SendEmailAsync_ValidParameter_Then_Success()
        {
            // Given
            SetBehavior();
            string email = EmailServiceTests.emailUser;
            string subject = EmailServiceTests.subject;
            string message = EmailServiceTests.message;

            // When
            var result = mockIEmailService.SendEmailAsync(email, subject, message);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeTrue();
            result.Result.Message.Should().Be(MessageSendEmailAsync);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_SendEmailAsync_InvalidSmtp_Then_Failed()
        {
            // Given
            SetBehavior(smtp: string.Empty);
            string email = EmailServiceTests.emailUser;
            string subject = EmailServiceTests.subject;
            string message = EmailServiceTests.message;

            // When
            var result = mockIEmailService.SendEmailAsync(email, subject, message);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be(MessageConfigurationInvalid);
            result.Result.Types.Should().Be(ErrorTypes.ConfigurationMissingError);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_SendEmailAsync_InvalidEmailAddress_Then_Failed()
        {
            // Given
            SetBehavior(emailAddress: string.Empty);
            string email = EmailServiceTests.emailUser;
            string subject = EmailServiceTests.subject;
            string message = EmailServiceTests.message;

            // When
            var result = mockIEmailService.SendEmailAsync(email, subject, message);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be(MessageConfigurationInvalid);
            result.Result.Types.Should().Be(ErrorTypes.ConfigurationMissingError);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_SendEmailAsync_InvalidPort_Then_Failed()
        {
            // Given
            SetBehavior(port: string.Empty);
            string email = EmailServiceTests.emailUser;
            string subject = EmailServiceTests.subject;
            string message = EmailServiceTests.message;

            // When
            var result = mockIEmailService.SendEmailAsync(email, subject, message);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be(MessageConfigurationInvalid);
            result.Result.Types.Should().Be(ErrorTypes.ConfigurationMissingError);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_SendEmailAsync_InvalidUsername_Then_Failed()
        {
            // Given
            SetBehavior(username: string.Empty);
            string email = EmailServiceTests.emailUser;
            string subject = EmailServiceTests.subject;
            string message = EmailServiceTests.message;

            // When
            var result = mockIEmailService.SendEmailAsync(email, subject, message);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be(MessageConfigurationInvalid);
            result.Result.Types.Should().Be(ErrorTypes.ConfigurationMissingError);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_SendEmailAsync_InvalidPassword_Then_Failed()
        {
            // Given
            SetBehavior(password: string.Empty);
            string email = EmailServiceTests.emailUser;
            string subject = EmailServiceTests.subject;
            string message = EmailServiceTests.message;

            // When
            var result = mockIEmailService.SendEmailAsync(email, subject, message);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be(MessageConfigurationInvalid);
            result.Result.Types.Should().Be(ErrorTypes.ConfigurationMissingError);
            return Task.CompletedTask;
        }

        private void SetConfigurationValues(string smtp, string emailAddress, string port, string username, string password)
        {

            mockConfigSectionSmtp
                .Setup(x => x.Value)
                .Returns(smtp);
            mockConfigSectionEmailAddress
                .Setup(x => x.Value)
                .Returns(emailAddress);
            mockConfigSectionUsername
                .Setup(x => x.Value)
                .Returns(username);
            mockConfigSectionPassword
                .Setup(x => x.Value)
                .Returns(password);
            mockConfigSectionPort
                .Setup(x => x.Value)
                .Returns(port);
            mockConfiguration
                .Setup(section => section.GetSection("email:smtp"))
                .Returns(mockConfigSectionSmtp.Object);
            mockConfiguration
                .Setup(section => section.GetSection("email:mailAddress"))
                .Returns(mockConfigSectionEmailAddress.Object);
            mockConfiguration
                .Setup(section => section.GetSection("email:username"))
                .Returns(mockConfigSectionUsername.Object);
            mockConfiguration
                .Setup(section => section.GetSection("email:password"))
                .Returns(mockConfigSectionPassword.Object);
            mockConfiguration
                .Setup(section => section.GetSection("email:port"))
                .Returns(mockConfigSectionPort.Object);
            mockIHttpContentWrapper
                .Setup(response => response.SendMailAsync(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()))
                .Returns(Task.FromResult(""));

        }
    }
}
