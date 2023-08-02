namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    public class ValidateUsernameUserRepository : BaseTests
    {
        [Test]
        public Task When_ValidateUsername_ValidParameters_Then_Success()
        {
            // Given
            var filter = string.Empty;

            // When
            var result = _userRepository.ValidateUsername("withoutemail@notserver.mail.com");

            // Then
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(-1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Message.Should().Be("The given username is not in a valid format");
            return Task.CompletedTask;
        }
    }
}