using AuthFlow.Domain.Entities;
using AuthFlow.Test.Infraestructure.Repository.BaseTest;
using FluentAssertions;

namespace AuthFlow.Test.Infraestructure.Repository
{
    [TestFixture]
    public class LoingUserRepositoryTests : BaseTests
    {
        private const string success = "The user was loging successfully.";
        private const string failedWrongPassword = "The given username or password is incorrect.";
        private const string failedWrongUser = "The user was not found - unable to create the session.";

        [Test]
        public async Task When_Login_ValidParameter_Then_Success()
        {
            // Given
            var key = "261c2aab-b974-4d41-b56c-6010247465c2";
            var userName = $"john.doe.{key}";
            var email = $"john.doe.{key}@example.com";
            var password = $"password.{key}";
            User user = GetUser(userName, email, password );
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Login(user.Username, user.Password);

            // Then
            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().NotBeNullOrEmpty();
            
        }

        [Test]
        public async Task When_Login_InvalidPassword_Then_Failed()
        {
            // Given
            var key = "261c2aab-4rfd-4d41-b56c-6010247465c2";
            var userName = $"john.doe.{key}";
            var email = $"john.doe.{key}@example.com";
            var password = $"password.{key}";
            User user = GetUser(userName, email, password);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Login(user.Username, user.Password + "PasswordWrong");

            // Then
            result?.Message.Should().Be(failedWrongPassword);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task When_Login_InvalidUsername_Then_Failed()
        {
            // Given
            var key = "261c2aab-4rfd-4d41-yu72-6010247465c2";
            var userName = $"john.doe.{key}";
            var email = $"john.doe.{key}@example.com";
            var password = $"password.{key}";
            User user = GetUser(userName, email, password);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Login(user.Username + "UserWrong", user.Password );

            // Then
            result?.Message.Should().Be(failedWrongUser);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeNullOrEmpty();
        }

    }
}