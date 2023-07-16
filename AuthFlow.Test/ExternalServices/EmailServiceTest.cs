using AuthFlow.Application.DTOs;
using AuthFlow.Application.Use_cases.Interface.Operations;
using AuthFlow.Test.RepositoryTests;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AuthFlow.Test.ExternalServices
{
    [TestFixture]
    public class EmailServiceTests : BaseTests
    {
        private Mock<IEmailService> _mockEmailService;
        private IEmailService _emailService;

        [SetUp]
        public void Setup()
        {
            _mockEmailService = new Mock<IEmailService>();
            _emailService = _mockEmailService.Object;
        }

        [Test]
        public async Task Given_ValidEmailParameters_When_SendEmailAsync_Then_ReturnsSuccessOperationResult()
        {
            // Given
            string email = "test@test.com";
            string subject = "Test Subject";
            string message = "Test Message";
            var operationResult = new OperationResult<bool> { IsSuccessful = true, Data = true };

            _mockEmailService
                .Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(operationResult);

            // When
            var result = await _emailService.SendEmailAsync(email, subject, message);

            // Then
            result.Should().BeEquivalentTo(operationResult);
            _mockEmailService.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Given_InvalidEmailParameters_When_SendEmailAsync_Then_ReturnsFailureOperationResult()
        {
            // Given
            string email = "invalid";
            string subject = "Test Subject";
            string message = "Test Message";
            var operationResult = new OperationResult<bool> { IsSuccessful = false, Data = false };

            _mockEmailService
                .Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(operationResult);

            // When
            var result = await _emailService.SendEmailAsync(email, subject, message);

            // Then
            result.Should().BeEquivalentTo(operationResult);
            _mockEmailService.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
