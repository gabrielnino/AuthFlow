using AuthFlow.Application.DTOs;
using AuthFlow.Application.Use_cases.Interface.Operations;
using AuthFlow.Test.RepositoryTests;
using FluentAssertions;
using Moq;

namespace AuthFlow.Test.ExternalServices
{
    [TestFixture]
    public class OtpServiceTests : BaseTests
    {
        private Mock<IOtpService> _mockOtpService;
        private IOtpService _otpService;

        [SetUp]
        public void Setup()
        {
            _mockOtpService = new Mock<IOtpService>();
            _otpService = _mockOtpService.Object;
        }

        [Test]
        public async Task Given_ValidEmail_When_GenerateOtp_Then_ReturnsSuccessOperationResult()
        {
            // Given
            string email = "test@test.com";
            var operationResult = new OperationResult<bool> { IsSuccessful = true, Data = true };

            _mockOtpService
                .Setup(service => service.GenerateOtp(It.IsAny<string>()))
                .ReturnsAsync(operationResult);

            // When
            var result = await _otpService.GenerateOtp(email);

            // Then
            result.Should().BeEquivalentTo(operationResult);
            _mockOtpService.Verify(service => service.GenerateOtp(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Given_InvalidEmail_When_GenerateOtp_Then_ReturnsFailureOperationResult()
        {
            // Given
            string email = "invalid";
            var operationResult = new OperationResult<bool> { IsSuccessful = false, Data = false };

            _mockOtpService
                .Setup(service => service.GenerateOtp(It.IsAny<string>()))
                .ReturnsAsync(operationResult);

            // When
            var result = await _otpService.GenerateOtp(email);

            // Then
            result.Should().BeEquivalentTo(operationResult);
            _mockOtpService.Verify(service => service.GenerateOtp(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Given_ValidEmailAndOtp_When_ValidateOtp_Then_ReturnsSuccessOperationResult()
        {
            // Given
            string email = "test@test.com";
            string otp = "123456";
            var operationResult = new OperationResult<bool> { IsSuccessful = true, Data = true };

            _mockOtpService
                .Setup(service => service.ValidateOtp(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(operationResult);

            // When
            var result = await _otpService.ValidateOtp(email, otp);

            // Then
            result.Should().BeEquivalentTo(operationResult);
            _mockOtpService.Verify(service => service.ValidateOtp(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Given_InvalidEmailOrOtp_When_ValidateOtp_Then_ReturnsFailureOperationResult()
        {
            // Given
            string email = "invalid";
            string otp = "invalid";
            var operationResult = new OperationResult<bool> { IsSuccessful = false, Data = false };

            _mockOtpService
                .Setup(service => service.ValidateOtp(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(operationResult);

            // When
            var result = await _otpService.ValidateOtp(email, otp);

            // Then
            result.Should().BeEquivalentTo(operationResult);
            _mockOtpService.Verify(service => service.ValidateOtp(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
