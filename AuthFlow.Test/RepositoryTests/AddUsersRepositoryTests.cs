using AuthFlow.Domain.Entities;
using FluentAssertions;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class AddUsersRepositoryTests : BaseTests
    {

        private const string alreadyRegisteredUsername = "A user is already registered with this username.";
        private const string success = "User was created successfully.";
        private const string necessaryData = "Necessary data was not provided.";
        private const string lengthMinimun = "One or more data from the User have been submitted with errors The length of 'Username' must be at least 6 characters. You entered 3 characters., The length of 'Password' must be at least 6 characters. You entered 3 characters., The length of 'Email' must be at least 10 characters. You entered 3 characters.";
        private const string lengthOverMaximum = "One or more data from the User have been submitted with errors The length of 'Username' must be 50 characters or fewer. You entered 111 characters., The length of 'Password' must be 100 characters or fewer. You entered 111 characters., The length of 'Email' must be 100 characters or fewer. You entered 111 characters.";
        private const string alreadyRegisteredEmail = "A user is already registered with this email.";
        private const string emailMustNotEmpty = "One or more data from the User have been submitted with errors 'Email' must not be empty.";
        private const string usernameMustNotEmpty = "One or more data from the User have been submitted with errors 'Username' must not be empty.";
        private const string passwordMustNotEmpty = "One or more data from the User have been submitted with errors 'Password' must not be empty.";

        [Test]
        public async Task Given_user_When_AddingUser_Then_SuccessResultWithIdReturned()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);

            // When
            var result = await _userRepository.Add(user);

            // Then
            result?.Message.Should().BeEquivalentTo(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task Given_user_null_email_When_AddingUser_Then_FailedResultWithoutIdReturned()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            var user = new User
            {
                Username = $"john.{name}",
                Password = "password",
            };
            
            // When
            var result = await _userRepository.Add(user);

            // Then
            result?.Message.Should().Be(emailMustNotEmpty);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }

        [Test]
        public async Task Given_user_null_username_When_AddingUser_Then_FailedResultWithoutIdReturned()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            var user = new User
            {
                Password = "password",
                Email = $"john.{name}@example.com",
            };

            // When
            var result = await _userRepository.Add(user);

            // Then
            result?.Message.Should().Be(usernameMustNotEmpty);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }

        [Test]
        public async Task Given_user_null_password_When_AddingUser_Then_FailedResultWithoutIdReturned()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            var user = new User
            {
                Username = $"john.{name}",
                Email = $"john.{name}@example.com",
            };

            // When
            var result = await _userRepository.Add(user);

            // Then
            result?.Message.Should().Be(passwordMustNotEmpty);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }

        [Test]
        public async Task Given_user_null_When_AddingUser_Then_FailedResultWithoutIdReturned()
        {
            // Given
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            User user = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // When
            var result = await _userRepository.Add(user);

            // Then
            result.Message.Should().Be(necessaryData);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }

        [Test]
        public async Task Given_user_min_lengt_When_AddingUser_Then_FailedResultWithoutIdReturned()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name, 4);

            // When
            var result = await _userRepository.Add(user);

            // Then
            result.Message.Should().Be(lengthMinimun);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }

        [Test]
        public async Task Given_user_max_lengt_When_AddingUser_Then_FailedResultWithoutIdReturned()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name, 0, 110);

            // When
            var result = await _userRepository.Add(user);

            // Then
            result.Message.Should().Be(lengthOverMaximum);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }

        [Test]
        public async Task Given_duplicate_user_When_AddingUser_Then__FailedResultWithoutIdReturned()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            _ = await _userRepository.Add(user);
            user.Email = $"john.{name}.jr@example.com";
            // When
            var result = await _userRepository.Add(user);

            // Then
            result?.Message.Should().Be(alreadyRegisteredUsername);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }

        [Test]
        public async Task Given_duplicate_email_When_AddingUser_Then__FailedResultWithoutIdReturned()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            _ = await _userRepository.Add(user);
            user.Username = $"john.{name}.jr";
            // When
            Application.DTOs.OperationResult<int>? result = await _userRepository.Add(user);

            // Then
            result?.Message.Should().Be(alreadyRegisteredEmail);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}