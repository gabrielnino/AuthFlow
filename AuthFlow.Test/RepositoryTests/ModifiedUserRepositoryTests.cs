using AuthFlow.Domain.Entities;
using FluentAssertions;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class ModifiedUserRepositoryTests : BaseTests
    {
        private const string success = "User was updated successfully.";
        private const string alreadyRegisteredUsername = "A user is already registered with this username.";
        private const string necessaryData = "Necessary data was not provided.";
        private const string lengthMinimunUsername = "One or more data from the User have been submitted with errors The length of 'Username' must be at least 6 characters. You entered 3 characters.";
        private const string lengthMinimunEmail = "One or more data from the User have been submitted with errors The length of 'Email' must be at least 10 characters. You entered 3 characters.";
        private const string lengthMinimunPassword = "One or more data from the User have been submitted with errors The length of 'Password' must be at least 6 characters. You entered 3 characters.";
        private const string lengthMinimunAllField = "One or more data from the User have been submitted with errors The length of 'Username' must be at least 6 characters. You entered 3 characters., The length of 'Password' must be at least 6 characters. You entered 3 characters., The length of 'Email' must be at least 10 characters. You entered 3 characters.";
        private const string lengthMaximunUsername = "One or more data from the User have been submitted with errors The length of 'Username' must be 50 characters or fewer. You entered 111 characters.";
        private const string lenghtMaximunEmail = "One or more data from the User have been submitted with errors The length of 'Email' must be 100 characters or fewer. You entered 111 characters.";
        private const string lenghtMaximunPassword = "One or more data from the User have been submitted with errors The length of 'Password' must be 100 characters or fewer. You entered 111 characters.";
        private const string lenghtAllField = "One or more data from the User have been submitted with errors The length of 'Username' must be 50 characters or fewer. You entered 111 characters., The length of 'Password' must be 100 characters or fewer. You entered 111 characters., The length of 'Email' must be 100 characters or fewer. You entered 111 characters.";
        private const string alreadyRegisteredEmail = "A user is already registered with this email.";
        private const string emailMustNotEmpty = "One or more data from the User have been submitted with errors 'Email' must not be empty.";
        private const string usernameMustNotEmpty = "One or more data from the User have been submitted with errors 'Username' must not be empty.";
        private const string passwordMustNotEmpty = "One or more data from the User have been submitted with errors 'Password' must not be empty.";
        
        [Test]
        public async Task Given_user_When_ModifiedUser_Then_SuccessResultWithTrue()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            var repoFind = await _userRepository.GetAllByFilter(u => u.Id == user.Id);
            var userFound = repoFind.Data.FirstOrDefault();
            // When
            userFound.Password = userFound.Password + "_Modified";
            var result = await _userRepository.Modified(userFound);

            // Then
            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Test]
        public async Task Given_user_null_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            User user = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(necessaryData);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_does_not_exist_Id_is_cero_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result?.Message.Should().Be(necessaryData);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }
        /*
        [Test]
        public async Task Given_user_does_not_exist_Id_is_diferent_cero_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            user.Id = 20;
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result?.Message.Should().Be(necessaryData);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }
        */
        [Test]
        public async Task Given_user_null_email_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            var user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Email = null;

            // When
            var result = await _userRepository.Modified(user);

            // Then
            result?.Message.Should().Be(emailMustNotEmpty);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }


        [Test]
        public async Task Given_user_null_username_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            var user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Username = null;

            // When
            var result = await _userRepository.Modified(user);

            // Then
            result?.Message.Should().Be(usernameMustNotEmpty);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_null_password_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            var user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Password = null;

            // When
            var result = await _userRepository.Modified(user);

            // Then
            result?.Message.Should().Be(passwordMustNotEmpty);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }


        [Test]
        public async Task Given_user_duplicate_username_When_ModifiedUser_Then_FailedResultWithFalse()
        { 
            // Given
            var name = Guid.NewGuid().ToString();
            var user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            var name_two = Guid.NewGuid().ToString();
            var user_two = GetUser(name_two);
            var repo_two = await _userRepository.Add(user_two);
            user_two.Id = repo_two.Data;
            user_two.Username = user.Username;

            // When
            var result = await _userRepository.Modified(user_two);

            // Then
            result?.Message.Should().Be(alreadyRegisteredUsername);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_duplicate_email_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            var user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            var name_two = Guid.NewGuid().ToString();
            var user_two = GetUser(name_two);
            var repo_two = await _userRepository.Add(user_two);
            user_two.Id = repo_two.Data;
            user_two.Email = user.Email;

            // When
            var result = await _userRepository.Modified(user_two);

            // Then
            result?.Message.Should().Be(alreadyRegisteredEmail);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_min_lengt_username_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Username = GetMinimumLength(4, user.Username);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(lengthMinimunUsername);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_min_lengt_email_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Email = GetMinimumLength(4, user.Email);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(lengthMinimunEmail);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_min_lengt_password_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Password = GetMinimumLength(4, user.Email);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(lengthMinimunPassword);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_min_lengt_allField_When_ModifiedUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Username = GetMinimumLength(4, user.Username);
            user.Email = GetMinimumLength(4, user.Email);
            user.Password = GetMinimumLength(4, user.Password);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(lengthMinimunAllField);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_max_lengt_username_When_AddingUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Username = GetMaximumLength(110, user.Username);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(lengthMaximunUsername);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_max_lengt_email_When_AddingUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Email = GetMaximumLength(110, user.Email);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(lenghtMaximunEmail);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_max_lengt_password_When_AddingUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Password = GetMaximumLength(110, user.Password);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(lenghtMaximunPassword);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }

        [Test]
        public async Task Given_user_max_lengt_allField_When_AddingUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            user.Password = GetMaximumLength(110, user.Password);
            user.Email = GetMaximumLength(110, user.Email);
            user.Username = GetMaximumLength(110, user.Username);
            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.Message.Should().Be(lenghtAllField);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }


        [Test]
        public async Task Given_user_When_ModifiedUserEmail_Then_SuccessResultWithTrue()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            var repoFind = await _userRepository.GetAllByFilter(u=>u.Id == user.Id);
            var userFound = repoFind.Data.FirstOrDefault();

            // When
            userFound.Email = userFound.Email + "_Modified";
            var result = await _userRepository.Modified(userFound);

            // Then
            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }
    }
}