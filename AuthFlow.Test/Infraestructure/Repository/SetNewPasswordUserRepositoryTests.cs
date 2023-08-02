namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    public class SetNewPasswordUserRepositoryTests : BaseTests
    {
        [Test]
        public Task When_SetNewPassword_ValidParameters_Then_Success()
        {
            // Given
            var filter = string.Empty;

            // When
            var result = _userRepository.SetNewPassword("withoutemail@notserver.mail.com", "123456");

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be("This email is not registered by any user.");
            return Task.CompletedTask;
        }

        [Test]
        public Task When_SetNewPassword_InvalidParameters_Then_Failed()
        {
            // Given
            var email = "withoutemail@notserver.mail.com";
            var password = "123456";

            // When
            var result = _userRepository.SetNewPassword(email, password);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.Message.Should().Be("This email is not registered by any user.");
            return Task.CompletedTask;
        }
    }
}
