using AuthFlow.Test.Infraestructure.Repository.BaseTest;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFlow.Test.Infraestructure.Repository
{
    [TestFixture]
    public class LoginOtpUserRepositoryTests : BaseTests
    {
        [Test]
        public Task When_LoginOtp_ValidParameters_Then_Success()
        {
            // Given
            var filter = string.Empty;

            // When
            var result = _userRepository.LoginOtp("withoutemail@notserver.mail.com","123456");

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            result.Result.Message.Should().Be("The user was not found - unable to create the session.");
            return Task.CompletedTask;
        }

        [Test]
        public Task When_LoginOtp_InvalidParameters_Then_Success()
        {
            // Given
            var filter = string.Empty;

            // When
            var result = _userRepository.LoginOtp("with123oute456mail@notserver.mail.com", "1AAA23456");

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            result.Result.Message.Should().Be("The user was not found - unable to create the session.");
            return Task.CompletedTask;
        }

        [Test]
        public Task When_LoginOtp_InvalidParameters02_Then_Success()
        {
            // Given
            var filter = string.Empty;

            // When
            var result = _userRepository.LoginOtp("witutenotserver.mail.com", "12EE56");

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            result.Result.Message.Should().Be("The user was not found - unable to create the session.");
            return Task.CompletedTask;
        }

        [Test]
        public Task When_LoginOtp_InvalidParameters03_Then_Success()
        {
            // Given
            var filter = string.Empty;

            // When
            var result = _userRepository.LoginOtp("witutenotSSSSr.mail.com", "1SSWE56");

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            result.Result.Message.Should().Be("The user was not found - unable to create the session.");
            return Task.CompletedTask;
        }
    }
}
