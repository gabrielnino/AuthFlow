using AuthFlow.Domain.Entities;
using FluentAssertions;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class RemoveUserRepositoryTests : BaseTests
    {
        private const string success = "User was deleted successfully.";
        private const string userTryingInDeleteDoesNotExist = "The User you are trying to delete does not exist.";

        [Test]
        public async Task Given_user_When_RemoveUser_Then_SuccessResultWithTrue()
        {
            // Given
            var name = "ee8f161f-9a33-4fa0-9f9b-5e47f196b662";
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Remove(user.Id);

            // Then
            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Test]
        public async Task Given_user_When_RemoveUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = "b00e69f1-a32f-4d28-be10-c3365b17bcef";
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Remove(999999);

            // Then
            result?.Message.Should().Be(userTryingInDeleteDoesNotExist);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }
    }
}