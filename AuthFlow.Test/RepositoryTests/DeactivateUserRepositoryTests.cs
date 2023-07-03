using AuthFlow.Domain.Entities;
using FluentAssertions;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class DeactivateUserRepositoryTests : BaseTests
    {
        private const string success = "User was disabled successfully.";
        private const string userTryingInactiveDoesNotExist = "The user you are trying to inactive does not exist.";

        [Test]
        public async Task Given_user_When_DeactivateUser_Then_SuccessResultWithTrue()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Deactivate(user.Id);

            var userRepo = await _userRepository.GetAllByFilter(u => u.Id.Equals(user.Id));
            var userFound = userRepo.Data.FirstOrDefault();

            // Then
            result?.Message.Should().Be(success);
            userFound.Active.Should().BeFalse();
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Test]
        public async Task Given_user_When_DeactivateUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Deactivate(999999);

            // Then
            result?.Message.Should().Be(userTryingInactiveDoesNotExist);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }
    }
}