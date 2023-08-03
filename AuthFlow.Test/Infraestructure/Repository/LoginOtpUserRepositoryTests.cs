namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    [TestFixture]
    public class LoginOtpUserRepositoryTests : BaseTests
    {
        private const string MessageFailed = "The user was not found - unable to create the session.";

        [Test]
        public Task When_LoginOtp_ValidParameters_Then_Success()
        {
            // Given
            var email = "withoutemail@notserver.mail.com";
            var password = "123456";

            // When
            var result = _userRepository.LoginOtp(email,password);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            result.Result.Message.Should().Be(MessageFailed);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_LoginOtp_InvalidParameters_Then_Success()
        {
            // Given
            var email = "with123oute456mail@notserver.mail.com";
            var password = "1AAA23456";

            // When
            var result = _userRepository.LoginOtp(email, password);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            result.Result.Message.Should().Be(MessageFailed);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_LoginOtp_InvalidParameters02_Then_Success()
        {
            // Given
            var email = "witutenotserver.mail.com";
            var password = "12EE56";

            // When
            var result = _userRepository.LoginOtp(email, password);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            result.Result.Message.Should().Be(MessageFailed);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_LoginOtp_InvalidParameters03_Then_Success()
        {
            // Given
            var email = "witutenotSSSSr.mail.com";
            var password = "1SSWE56";

            // When
            var result = _userRepository.LoginOtp(email, password);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNull();
            result.Result.Message.Should().Be(MessageFailed);
            return Task.CompletedTask;
        }
    }
}
