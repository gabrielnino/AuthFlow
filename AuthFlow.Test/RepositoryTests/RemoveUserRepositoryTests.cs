using AuthFlow.Application.Uses_cases.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Persistence.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

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
            var name = Guid.NewGuid().ToString();
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
            var name = Guid.NewGuid().ToString();
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