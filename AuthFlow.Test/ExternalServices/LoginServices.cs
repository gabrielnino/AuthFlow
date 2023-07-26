using AuthFlow.Application.DTOs;
using AuthFlow.Application.Use_cases.Interface.Operations;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFlow.Test.ExternalServices
{
    internal class LoginServices
    {
        private Mock<ILoginServices> _mockOtpService;
        private ILoginServices _otpService;

        [SetUp]
        public void Setup()
        {
            _mockOtpService = new Mock<ILoginServices>();
            _otpService = _mockOtpService.Object;
        }

        [Test]
        public async Task Given_LoginOtp_When_ValidateOtp_Then_ReturnsSuccessOperationResult()
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
    }
}
