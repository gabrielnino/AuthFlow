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
        private const string invalidEmailFormat = "The given email is not in a valid format";

        [Test]
        public async Task Given_user_When_AddingUser_Then_SuccessResultWithIdReturned()
        {
            // Given
            var name = "549f726e-3fe5-4f87-af14-95d36e5ee9d8";
            User user = GetUser(name);

            // When
            var result = await _userRepository.Add(user);

            // Then
            result?.Message.Should().BeEquivalentTo(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task Given_user_password_When_AddingUser_Then_SuccessResultWithIdReturned()
        {
            // Given
            var name = "83997f50-1748-4991-9d9b-2d96e1337db9";
            User user = GetUser(name);
            var passwordhash = ComputeSha256Hash(user.Password);
            // When
            var result = await _userRepository.Add(user);
            user.Id = result.Data;
            var resultRepo = await _userRepository.GetAllByFilter(u => u.Id.Equals(user.Id));
            var userCompute = resultRepo.Data.FirstOrDefault();
            // Then
            result?.Message.Should().BeEquivalentTo(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeGreaterThan(0);
            userCompute.Password.Should().Be(passwordhash);
        }

        [Test]
        public async Task Given_user_null_email_When_AddingUser_Then_FailedResultWithoutIdReturned()
        {
            // Given
            var name = "5309395a-ab03-45c1-bdf6-d85fe9f2b967";
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
            var name = "559b767c-162c-49a6-acf7-90fb600a5d27";
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
            var name = "70e5c921-5c33-40cf-b859-bb85f6fa7def";
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
            var name = "cd391035-0927-45d5-826f-72b41fb1fcc8";
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
            var name = "ac2d4ea5-5d48-49ce-9d8e-04438798abb6";
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
            var name = "2485eac5-7c39-4c66-b324-facb0915fb89";
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
            var name = "26ce5a07-ddcb-4026-b229-eaa0df6c3597";
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


        [Test]
        public async Task Given_user_When_AddingUser_Then_FaildedByEmailResultWithIdReturned()
        {
            // Given
            var name = "549f726e-3fe5-4f87-af14-9r35te5ee9d8";
            User user = GetUser(name);
            user.Email = "not_is_an_email";
            // When
            var result = await _userRepository.Add(user);

            // Then
            result?.Message.Should().BeEquivalentTo(invalidEmailFormat);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}