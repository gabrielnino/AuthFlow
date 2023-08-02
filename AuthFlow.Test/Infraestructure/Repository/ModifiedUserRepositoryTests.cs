namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

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
        private const string invalidEmailFormat = "The given email is not in a valid format";

        [Test]
        public Task When_Modified_ValidUser_Then_Success()
        {
            // Given
            var name = "89855be1-46a0-4aa0-873f-92ed6234f191";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            var repoFind = _userRepository.GetAllByFilter(u => u.Id == user.Id);
            var userFound = repoFind.Result.Data.FirstOrDefault();
            // When
            userFound.Password = userFound.Password + "_Modified";
            var result = _userRepository.Modified(userFound);

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
        public Task When_Modified_InvalidUser_Then_Failed()
        {
            // Given
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            User user = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(necessaryData);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_InvalidUserData_Then_Failed()
        {
            // Given
            var name = "37ab864d-d9c4-47c5-9d3b-406d5267c1a6";
            User user = GetUser(name);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(necessaryData);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }
        
        [Test]
        public Task When_Modified_InvalidUserData02_Then_Failed()
        {
            // Given
            var name = "ee7c4716-9db7-49a1-955e-72e2a65ec0d4";
            User user = GetUser(name);
            user.Id = 999999999;
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(necessaryData);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }
        
        [Test]
        public Task When_Modified_InvalidEmailEmpty_Then_Failed()
        {
            // Given
            var name = "11407931-6c9e-4dff-b4db-33a9dbf6d1f8";
            var user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Email = null;

            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(emailMustNotEmpty);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }


        [Test]
        public Task When_Modified_InvalidUsernameEmpty_Then_Failed()
        {
            // Given
            var name = "5ded9739-cf32-4027-856c-c77bb6b74522";
            var user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Username = null;

            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(usernameMustNotEmpty);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_InvalidPasswordEmpty_Then_Failed()
        {
            // Given
            var name = "29843786-3057-4388-8a20-8445e8947075";
            var user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Password = null;

            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(passwordMustNotEmpty);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }


        [Test]
        public Task When_Modified_AlreadyRegistedUsername_Then_Failed()
        { 
            // Given
            var name = "0e29703d-f110-4478-ab5f-f8ef5f9f9de7";
            var user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;

            var name_two = Guid.NewGuid().ToString();
            var user_two = GetUser(name_two);
            var repo_two = _userRepository.Add(user_two);
            user_two.Id = repo_two.Result.Data;
            user_two.Username = user.Username;

            // When
            var result = _userRepository.Modified(user_two);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(alreadyRegisteredUsername);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_AlreadyRegisteredEmail_Then_Failed()
        {
            // Given
            var name = "46b28fe8-65ab-4279-a290-8f8e8aa3177d";
            var user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;

            var name_two = Guid.NewGuid().ToString();
            var user_two = GetUser(name_two);
            var repo_two = _userRepository.Add(user_two);
            user_two.Id = repo_two.Result.Data;
            user_two.Email = user.Email;

            // When
            var result =  _userRepository.Modified(user_two);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(alreadyRegisteredEmail);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_LenghMinimunUsername_Then_Failed()
        {
            // Given
            var name = "e7f32b4a-4cd3-4a39-8e49-414535cd9324";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Username = GetMinimumLength(4, user.Username);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(lengthMinimunUsername);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_LenghMinimunEmail_Then_Failed()
        {
            // Given
            var name = "e6297940-bb05-4b27-855d-9d03a7e2fc8c";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Email = GetMinimumLength(4, user.Email);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(lengthMinimunEmail);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_LenghMinimunPassword_Then_Failed()
        {
            // Given
            var name = "467cf2d3-85e7-4222-a1f8-8163683496f2";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Password = GetMinimumLength(4, user.Email);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(lengthMinimunPassword);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_LenghMinimunAllField_Then_Failed()
        {
            // Given
            var name = "0ce44259-b7c4-4b02-8463-b30b6920f66e";
            User user = GetUser(name);
            var repo =  _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Username = GetMinimumLength(4, user.Username);
            user.Email = GetMinimumLength(4, user.Email);
            user.Password = GetMinimumLength(4, user.Password);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(lengthMinimunAllField);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_LenghMaximunUsername_Then_Failed()
        {
            // Given
            var name = "537a68b3-ecb9-4633-81e0-e05bc7179635";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Username = GetMaximumLength(110, user.Username);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(lengthMaximunUsername);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_LenghMaximunEmail_Then_Failed()
        {
            // Given
            var name = "b0aff042-7f20-444c-8d4b-b01f437a8c91";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Email = GetMaximumLength(110, user.Email);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(lenghtMaximunEmail);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_LenghMaximunPassword_Then_Failed()
        {
            // Given
            var name = "49af61ca-6b87-4949-a451-d29a90819623";
            User user = GetUser(name);
            var repo =  _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Password = GetMaximumLength(110, user.Password);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(lenghtMaximunPassword);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Modified_LenghOverMaximunAlllFields_Then_Failed()
        {
            // Given
            var name = "cf68f306-05cc-40e2-80c2-d9551323f3f4";
            User user = GetUser(name);
            var repo = _userRepository.Add(user);
            user.Id = repo.Result.Data;
            user.Password = GetMaximumLength(110, user.Password);
            user.Email = GetMaximumLength(110, user.Email);
            user.Username = GetMaximumLength(110, user.Username);
            // When
            var result = _userRepository.Modified(user);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(lenghtAllField);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeFalse();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }


        [Test]
        public Task When_Modified_ValidUser02_Then_Success()
        {
            // Given
            var name = "ef0fac3f-61ea-4efe-b59a-9e53a735f3e0";
            User user = GetUser(name);
            var repo =  _userRepository.Add(user);
            user.Id = repo.Result.Data;
            var repoFind =  _userRepository.GetAllByFilter(u=>u.Id == user.Id);
            var userFound = repoFind.Result.Data.FirstOrDefault();

            // When
            userFound.Email = userFound.Email + "_Modified";
            var result =  _userRepository.Modified(userFound);

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
        public Task When_Modified_InvalidEmailFormat_Then_Success()
        {
            // Given
            var name = "89855be1-46a0-4aa0-873f-92ed623oiuhj";
            User user = GetUser(name);
            var repo =  _userRepository.Add(user);
            user.Id = repo.Result.Data;
            var repoFind =  _userRepository.GetAllByFilter(u => u.Id == user.Id);
            var userFound = repoFind.Result.Data.FirstOrDefault();
            // When
            userFound.Password = userFound.Password + "_Modified";
            userFound.Email = "not_is_email_Modified";
            var result = _userRepository.Modified(userFound);

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(invalidEmailFormat);
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