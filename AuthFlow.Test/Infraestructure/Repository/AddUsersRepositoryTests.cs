namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Domain.Entities;
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    [TestFixture]
    public class AddUsersRepositoryTests : BaseTests
    {

        private const string alreadyRegisteredUsername = "A user is already registered with this username.";
        private const string success = "User was created successfully.";
        private const string necessaryData = "Necessary data was not provided.";
        private const string lengthMinimun = "One or more data from the User have been submitted with errors The length of 'Username' must be at least 6 characters. You entered 3 characters., The length of 'Password' must be at least 6 characters. You entered 3 characters., The length of 'Email' must be at least 10 characters. You entered 3 characters.";
        private const string lengthOverMaximum = "One or more data from the User have been submitted with errors The length of 'Username' must be 50 characters or fewer. You entered 111 characters., The length of 'Password' must be 100 characters or fewer. You entered 111 characters., The length of 'Email' must be 100 characters or fewer. You entered 111 characters.";
        private const string alreadyRegisteredEmail = "A user is already registered with this email.";
        private const string usernameMustNotEmpty = "One or more data from the User have been submitted with errors 'Username' must not be empty.";
        private const string passwordMustNotEmpty = "One or more data from the User have been submitted with errors 'Password' must not be empty.";
        private const string invalidEmailFormat = "The given email is not in a valid format";
        private const string lengMinimumEmail = "One or more data from the User have been submitted with errors 'Email' must not be empty.";


        [Test]
        public Task When_Add_ValidParameter_Then_Success()
        {
            // Given
            var name = "549f726e-3fe5-4f87-af14-95d36e5ee9d8";
            User user = GetUser(name);

            // When
            var result =  _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeGreaterThan(0);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_ValidParameter_Then_DoubleCheck_Success()
        {
            // Given
            var name = "83997f50-1748-4991-9d9b-2d96e1337db9";
            User user = GetUser(name);
            var passwordhash = ComputeSha256Hash(user.Password);

            // When
            var result =  _userRepository.Add(user);
            user.Id = result.Result.Data;
            var resultRepo =  _userRepository.GetAllByFilter(u => u.Id.Equals(user.Id));
            var userCompute = resultRepo.Result.Data.FirstOrDefault();

            // Then
            userCompute.Password.Should().Be(passwordhash);
            UtilTest<int>.Assert(result);
            result.Result.Data.Should().BeGreaterThan(0);
            result.Result.IsSuccessful.Should().BeTrue(); 
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_InvalidEmail_Then_Failed()
        {
            // Given
            var name = "5309395a-ab03-45c1-bdf6-d85fe9f2b967";
            var user = new User
            {
                Username = $"john.{name}",
                Password = "password",
            };
            
            // When
            var result =  _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(lengMinimumEmail);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeGreaterThan(-1);

            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_InvalidUsername_Then_Failed()
        {
            // Given
            var name = "559b767c-162c-49a6-acf7-90fb600a5d27";
            var user = new User
            {
                Password = "password",
                Email = $"john.{name}@example.com",
            };

            // When
            var result =  _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(usernameMustNotEmpty);
            result.Result.Data.Should().BeGreaterThan(-1);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_InvalidPassword_Then_Failed()
        {
            // Given
            var name = "70e5c921-5c33-40cf-b859-bb85f6fa7def";
            var user = new User
            {
                Username = $"john.{name}",
                Email = $"john.{name}@example.com",
            };

            // When
            var result =  _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(passwordMustNotEmpty);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeGreaterThan(-1);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_InvalidUser_Then_Failed()
        {
            // Given
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            User user = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // When
            var result =  _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(necessaryData);
            result.Result.Data.Should().BeGreaterThan(-1);
            result.Result.ErrorType.Should().Be(ErrorTypes.BusinessValidationError);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_InvalidUserLength4_Then_Failed()
        {
            // Given
            var name = "cd391035-0927-45d5-826f-72b41fb1fcc8";
            User user = GetUser(name, 4);

            // When
            var result = _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(lengthMinimun);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeGreaterThan(-1);
            result.Result.ErrorType.Should().Be(ErrorTypes.BusinessValidationError);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_InvalidUserLength110_Then_Failed()
        {
            // Given
            var name = "ac2d4ea5-5d48-49ce-9d8e-04438798abb6";
            User user = GetUser(name, 0, 110);

            // When
            var result = _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(lengthOverMaximum);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeGreaterThan(-1);
            result.Result.ErrorType.Should().Be(ErrorTypes.BusinessValidationError);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_InvalidUsernameAlreadyRegister_Then_Failed()
        {
            // Given
            var name = "2485eac5-7c39-4c66-b324-facb0915fb89";
            User user = GetUser(name);
            _ =  _userRepository.Add(user);
            user.Email = $"john.{name}.jr@example.com";
            // When
            var result = _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(alreadyRegisteredUsername);
            result.Result.Data.Should().BeGreaterThan(-1);
            result.Result.ErrorType.Should().Be(ErrorTypes.BusinessValidationError);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_Add_InvalidEmailAlreadyRegister_Then_Failed()
        {
            // Given
            var name = "26ce5a07-ddcb-4026-b229-eaa0df6c3597";
            User user = GetUser(name);
            _ =  _userRepository.Add(user);
            user.Username = $"john.{name}.jr";
            // When
            var result = _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(alreadyRegisteredEmail);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeGreaterThan(-1);
            result.Result.ErrorType.Should().Be(ErrorTypes.BusinessValidationError);
            return Task.CompletedTask;
        }


        [Test]
        public Task When_Add_InvalidEmailInvalid_Then_Failed()
        {
            // Given
            var name = "549f726e-3fe5-4f87-af14-9r35te5ee9d8";
            User user = GetUser(name);
            user.Email = "not_is_an_email";
            // When
            var result = _userRepository.Add(user);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(invalidEmailFormat);
            result.Result.IsSuccessful.Should().BeFalse();
            result.Result.Data.Should().BeGreaterThan(-1);
            result.Result.ErrorType.Should().Be(ErrorTypes.BusinessValidationError);
            return Task.CompletedTask;
        }
    }
}