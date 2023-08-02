namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;


    [TestFixture]
    public class RemoveUserRepositoryTests : BaseTests
    {
        private const string success = "User was deleted successfully.";
        private const string userTryingInDeleteDoesNotExist = "The User you are trying to delete does not exist.";

        [Test]
        public Task When_Remove_ValidUser_Then_Success()
        {
            // Given
            var name = "ee8f161f-9a33-4fa0-9f9b-5e47f196b662";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result = _userRepository.Remove(user.Id);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(success);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeTrue();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Remove_InvalidId_Then_Failed()
        {
            // Given
            var name = "b00e69f1-a32f-4d28-be10-c3365b17bcef";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result = _userRepository.Remove(999999);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(userTryingInDeleteDoesNotExist);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }
    }
}