namespace AuthFlow.Test.Infraestructure.Operations
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using AuthFlow.Application.Use_cases.Interface.Operations;
    using AuthFlow.Infraestructure.Operations;
    using FluentAssertions;
    using Microsoft.Extensions.Caching.Distributed;
    using Moq;

    public class OTPServicesTests
    {
        private const string MessageEmailSuccessfully = "The email was sent successfully.";
        private const string MessageGenerateOTPSuccessfully = "The OTP code was generated successfully.";
        private const string email = "test@test.com";
        private const string otp = "123456";
        private const string MessageOTPFailed = "The OTP was not submitted.";
        private const string MessageInvalidOtpFailed = "The OTP does not exist.";
        private IOTPServices mockIOtpService;
        private Mock<IDistributedCache> mockIDistributedCache;
        private Mock<IEmailService> mockIEmailService;
        private Mock<ILogService> mocklogService;

        [SetUp]
        public void Setup()
        {
            mockIDistributedCache = new Mock<IDistributedCache>();
            mockIEmailService = new Mock<IEmailService>();
            mocklogService = new Mock<ILogService>();
        }

        private void SetBehavior(OperationResult_REVIEWED<bool>? success = null, string? otp = null)
        {
            var result = success ?? OperationResult_REVIEWED<bool>.Success(true, MessageEmailSuccessfully);
            var otpSend =  otp ?? OTPServicesTests.otp;
            SetConfigurationValues(result, otpSend);
            mockIOtpService = new OtpService(mockIDistributedCache.Object, mockIEmailService.Object, mocklogService.Object);
        }

        [Test]
        public Task When_GenerateOtp_ValidEmail_Then_Success()
        {
            // Given
            SetBehavior();
            string email = OTPServicesTests.email;

            // When
            var result = mockIOtpService.GenerateOtp(email);

            // Then
            UtilTest<bool>.Assert(result);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeTrue();
            result.Result.Message.Should().Be(MessageGenerateOTPSuccessfully);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_GenerateOtp_SendEmailFailed_Then_Failed()
        {
            // Given
            var resultSendEnmailFailed = OperationResult_REVIEWED<bool>.FailureExtenalService("FailureExtenalService");
            SetBehavior(resultSendEnmailFailed);
            string email = OTPServicesTests.email;

            // When
            var result = mockIOtpService.GenerateOtp(email);

            // Then
            UtilTest<bool>.Assert(result);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be("FailureExtenalService");
            var expected = ErrorTypes_REVIEWED.BusinessValidationError.ToErrorString();
            result.Result.Error.Should().Be(expected);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_ValidateOtp_InvalidParameter_Then_Success()
        {
            // Given
            SetBehavior();
            string email = OTPServicesTests.email;
            string otp = string.Empty; 

            // When
            var result = mockIOtpService.ValidateOtp(email, otp);

            // Then
            UtilTest<bool>.Assert(result);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be(MessageOTPFailed);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_ValidateOtp_InvalidOTP_Then_Failed()
        {
            // Given
            SetBehavior(otp: string.Empty);
            string email = string.Empty;
            string otp = OTPServicesTests.otp;

            // When
            var result = mockIOtpService.ValidateOtp(email, otp);

            // Then
            UtilTest<bool>.Assert(result);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be(MessageInvalidOtpFailed);
            var expected = ErrorTypes_REVIEWED.BusinessValidationError.ToErrorString();
            result.Result.Error.Should().Be(expected);
            return Task.CompletedTask;
        }

        private void SetConfigurationValues(OperationResult_REVIEWED<bool> success, string otpResponse)
        {
            mockIEmailService
                .Setup(x => x.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(success));

            byte[] otpBytes = System.Text.Encoding.UTF8.GetBytes(otpResponse);
            mockIDistributedCache
                .Setup(x => x.Get(It.IsAny<string>()))
                .Returns(otpBytes);
        }
    }
}
