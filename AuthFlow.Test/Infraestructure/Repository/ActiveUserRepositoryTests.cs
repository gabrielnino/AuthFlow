namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Domain.Entities;
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    [TestFixture]
    public class ActiveUserRepositoryTests : BaseTests
    {
        private const string success = "User was activated successfully.";
        private const string userTryingActiveDoesNotExist = "The User you are trying to active does not exist.";


        [Test]
        public Task When_Activate_ValidParameter_Then_Success()
        {
            // Given
            var name = "77e167cc-2340-46ef-b333-5e4aa57aaccc";
            User user = GetUser(name);
            var repo =  _userRepository.Add(user);
            user.Id =  repo.Result.Data;

            // When
            var result =  _userRepository.Activate(user.Id);
            var userRepo =  _userRepository.GetAllByFilter(u => u.Id.Equals(user.Id));
            var userFound = userRepo.Result.Data.FirstOrDefault();

            // Then
            UtilTest<bool>.Assert(result);
            userFound.Active.Should().BeTrue();
            result.Result.Message.Should().Be(success);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeTrue();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Activate_InvalidParameter_Then_Failed()
        {
            // Given
            var name = "ab7c2296-bf1e-4f0a-9d5a-35ad767d0d12";
            User user = GetUser(name);
            var repo =  _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result =  _userRepository.Activate(99999999);

            // Then
            UtilTest<bool>.Assert(result);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Result.ErrorType.Should().Be(ErrorTypes.BusinessValidationError);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            return Task.CompletedTask;
        }
    }
}