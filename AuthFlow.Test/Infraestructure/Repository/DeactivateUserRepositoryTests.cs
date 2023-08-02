namespace AAuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    [TestFixture]
    public class DeactivateUserRepositoryTests : BaseTests
    {
        private const string success = "User was disabled successfully.";
        private const string userTryingInactiveDoesNotExist = "The user you are trying to inactive does not exist.";

        [Test]
        public Task When_Deactivate_ValidParameters_Then_Success()
        {
            // Given
            var name = "1e8a562d-1a7f-4848-9bee-a82b438dfe4b";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result = _userRepository.Deactivate(user.Id);

            var userRepo = _userRepository.GetAllByFilter(u => u.Id.Equals(user.Id));
            var userFound = userRepo.Result.Data.FirstOrDefault();

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
        public Task When_Deactivate_InvalidId_Then_Failed()
        {
            // Given
            var name = "5a910d91-68d9-400d-b093-2519ed4701a3";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result = _userRepository.Deactivate(999999);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(userTryingInactiveDoesNotExist);
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