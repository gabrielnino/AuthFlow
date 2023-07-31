using AuthFlow.Domain.Entities;
using FluentAssertions;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class ActiveUserRepositoryTests : BaseTests
    {
        private const string success = "User was activated successfully.";
        private const string userTryingActiveDoesNotExist = "The User you are trying to active does not exist.";


        [Test]
        public async Task Given_user_When_ActivedUser_Then_SuccessResultWithTrue()
        {
            // Given
            var name = "77e167cc-2340-46ef-b333-5e4aa57aaccc";
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Activate(user.Id);

            var userRepo = await _userRepository.GetAllByFilter(u => u.Id.Equals(user.Id));
            var userFound = userRepo.Data.FirstOrDefault();
            // Then
            result?.Message.Should().Be(success);
            userFound.Active.Should().BeTrue();
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Test]
        public async Task Given_user_When_ActivedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = "ab7c2296-bf1e-4f0a-9d5a-35ad767d0d12";
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Activate(99999999);

            // Then
            result?.Message.Should().Be(userTryingActiveDoesNotExist);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }
    }
}