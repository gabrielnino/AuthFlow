using AuthFlow.Domain.Entities;
using AuthFlow.Test.Infraestructure.Repository.BaseTest;
using FluentAssertions;

namespace AAuthFlow.Test.Infraestructure.Repository
{
    [TestFixture]
    public class DeactivateUserRepositoryTests : BaseTests
    {
        private const string success = "User was disabled successfully.";
        private const string userTryingInactiveDoesNotExist = "The user you are trying to inactive does not exist.";

        [Test]
        public async Task When_Deactivate_ValidParameters_Then_Success()
        {
            // Given
            var name = "1e8a562d-1a7f-4848-9bee-a82b438dfe4b";
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
        public async Task When_Deactivate_InvalidId_Then_Failed()
        {
            // Given
            var name = "5a910d91-68d9-400d-b093-2519ed4701a3";
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