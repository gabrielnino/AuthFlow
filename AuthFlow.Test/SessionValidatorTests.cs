using NUnit.Framework;
using AuthFlow.Application.Validators;
using AuthFlow.Domain.Entities;

namespace AuthFlow.Application.Tests.Validators
{
    [TestFixture]
    public class SessionValidatorTests
    {
        [Test]
        public void GivenValidSession_WhenValidating_ThenValidationSucceeds()
        {
            // Given
            var session = new Session
            {
                Id = 1,
                UserId = 1,
                Token = "abcd1234",
                Expiration = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Active = true
            };

            var validator = new SessionValidator();

            // When
            var result = validator.Validate(session);

            // Then
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GivenSessionWithInvalidToken_WhenValidating_ThenValidationFails()
        {
            // Given
            var session = new Session
            {
                Id = 1,
                UserId = 1,
                Token = null, // Invalid token (null)
                Expiration = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Active = true
            };

            var validator = new SessionValidator();

            // When
            var result = validator.Validate(session);

            // Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Token' must not be empty.", result.Errors[0].ErrorMessage);
        }

        [Test]
        public void GivenSessionWithExpiredExpiration_WhenValidating_ThenValidationFails()
        {
            // Given
            var expecteExpiration = DateTime.UtcNow.AddDays(-1);
            var session = new Session
            {
                Id = 1,
                UserId = 1,
                Token = "abcd1234",
                Expiration = expecteExpiration, // Expired expiration
                CreatedAt = DateTime.UtcNow,
                Active = true
            };

            var validator = new SessionValidator();

            // When
            var result = validator.Validate(session);

            // Then
            Assert.IsFalse(result.IsValid);
            var expectedExpirationString = $"'Expiration' must be greater than '{expecteExpiration.AddDays(1).ToString("dd/MM/yyyy hh:mm:ss tt")}'.";
            Assert.AreEqual(expectedExpirationString, result.Errors[0].ErrorMessage);
        }
    }
}
