namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    [TestFixture]
    public class LoingUserRepositoryTests : BaseTests
    {
        private const string success = "The user was loging successfully.";
        private const string failedWrongPassword = "The given username or password is incorrect.";
        private const string failedWrongUser = "The user was not found - unable to create the session.";
        private const string necessaryData = "Necessary data was not provided.";

        [Test]
        public Task When_Login_ValidParameter_Then_Success()
        {
            // Given
            var key = "261c2aab-b974-4d41-b56c-6010247465c2";
            var userName = $"john.doe.{key}";
            var email = $"john.doe.{key}@example.com";
            var password = $"password.{key}";
            var user = GetUser(userName, email, password );
            var repo =  _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result = _userRepository.Login(user.Username, user.Password);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(success);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().NotBeNullOrEmpty();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;

        }

        [Test]
        public Task When_Login_InvalidPassword_Then_Failed()
        {
            // Given
            var key = "261c2aab-4rfd-4d41-b56c-6010247465c2";
            var userName = $"john.doe.{key}";
            var email = $"john.doe.{key}@example.com";
            var password = $"password.{key}";
            User user = GetUser(userName, email, password);
            var repo =  _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result =  _userRepository.Login(user.Username, user.Password + "PasswordWrong");

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(failedWrongPassword);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNullOrEmpty();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Login_InvalidUsername_Then_Failed()
        {
            // Given
            var key = "261c2aab-4rfd-4d41-yu72-6010247465c2";
            var userName = $"john.doe.{key}";
            var email = $"john.doe.{key}@example.com";
            var password = $"password.{key}";
            User user = GetUser(userName, email, password);
            var repo =  _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result = _userRepository.Login(user.Username + "UserWrong", user.Password );

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(failedWrongUser);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNullOrEmpty();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Login_InvalidParameters_Then_Failed()
        {
            // Given
            var key = "261c2aab-4rfd-4d41-yu72-6010247465c2";
            var userName = $"john.doe.{key}";
            var email = $"john.doe.{key}@example.com";
            var password = $"password.{key}";
            User user = GetUser(userName, email, password);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;

            // When
            var result = _userRepository.Login(string.Empty, string.Empty);

            UtilTest<string>.Assert(result);
            result.Result.Message.Should().Be(necessaryData);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeNullOrEmpty();
            return Task.CompletedTask;
        }

    }
}