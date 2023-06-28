using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Persistence.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AuthFlow.Tests.Infraestructure.Repositories
{
    [TestFixture]
    public class UsersRepositoryTests
    {
        private UsersRepository _userRepository;
        private AuthFlowDbContext _dbContextMock;
        //private Mock<IUserRepository> _userRepositoryMock;
        private DbContextOptions<AuthFlowDbContext> _options;

        [SetUp]
        public void Setup()
        {

             _options = new DbContextOptionsBuilder<AuthFlowDbContext>()
                .UseInMemoryDatabase(databaseName: "testdb")
                .Options;
            _dbContextMock =  new AuthFlowDbContext(_options);
            // Initialize the UsersRepository with the mocked dependencies
            _userRepository = new UsersRepository(_dbContextMock);
        }

        #region Given: A valid user entity

        [Test]
        public async Task Given_ValidUserEntity_When_AddingUser_Then_SuccessResultWithIdReturned()
        {
            User user = GetUser();

            // When
            var result = await _userRepository.Add(user);

            // Then
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().Be(user.Id);
        }

        private static User GetUser()
        {
            // Given
            return new User
            {
                Id = 1,
                Username = "john.doe",
                Email = "john.doe@example.com",
                Password = "password",
                Active = true
            };
        }

        [Test]
        public async Task Given_ValidUserEntity_When_ModifyingUser_UserDoesNotExist_Then_SuccessResultWithFalseReturned()
        {
            // Given
            var user = new User
            {
                Id = 1,
                Username = "john.doe",
                Email = "john.doe@example.com",
                Password = "password",
                Active = true
            };

            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
            result.Message.Should().NotBeEmpty();
        }
        /*
        [Test]
        public async Task Given_ValidUserEntity_When_DeactivatingUser_Then_SuccessResultWithTrueReturned()
        {
            // Given
            var userId = 1;

            // When
            var result = await _userRepository.Deactivate(userId);

            // Then
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Test]
        public async Task Given_ValidUserEntity_When_ActivatingUser_Then_SuccessResultWithTrueReturned()
        {

            // Given
            var userId = 1;

            // When
            var result = await _userRepository.Activate(userId);

            // Then
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Test]
        public async Task Given_ValidUserEntity_When_RemovingUser_Then_SuccessResultWithTrueReturned()
        {
            // Given
            var user = new User
            {
                Id = 1,
                Username = "john.doe",
                Email = "john.doe@example.com",
                Password = "password",
                Active = true
            };

            // When
            var result = await _userRepository.Remove(user.Id);

            // Then
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        #endregion

        #region Given: An invalid user entity

        [Test]
        public async Task Given_InvalidUserEntity_When_AddingUser_Then_FailureResultWithErrorMessageReturned()
        {
            // Given
            var user = new User
            {
                Id = 1,
                Username = "john.doe",
                Email = "john.doe@example.com",
                Password = "pass",
                Active = true
            };

            // When
            var result = await _userRepository.Add(user);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_InvalidUserEntity_When_ModifyingUser_Then_FailureResultWithErrorMessageReturned()
        {
            // Given
            var user = new User
            {
                Id = 1,
                Username = "john.doe",
                Email = "john.doe@example.com",
                Password = "pass",
                Active = true
            };

            // When
            var result = await _userRepository.Modified(user);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_InvalidUserEntity_When_DeactivatingUser_Then_FailureResultWithErrorMessageReturned()
        {
            // Given
            var userId = 1;

            // When
            var result = await _userRepository.Deactivate(userId);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_InvalidUserEntity_When_ActivatingUser_Then_FailureResultWithErrorMessageReturned()
        {
            // Given
            var userId = 1;

            // When
            var result = await _userRepository.Activate(userId);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_InvalidUserEntity_When_RemovingUser_Then_FailureResultWithErrorMessageReturned()
        {
            // Given
            var user = new User
            {
                Id = 1,
                Username = "john.doe",
                Email = "john.doe@example.com",
                Password = "pass",
                Active = true
            };

            // When
            var result = await _userRepository.Remove(user.Id);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Given: Non-existent user ID

        [Test]
        public async Task Given_NonExistentUserId_When_DeactivatingUser_Then_FailureResultWithErrorMessageReturned()
        {
            // Given
            var nonExistentUserId = 999;

            // When
            var result = await _userRepository.Deactivate(nonExistentUserId);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_NonExistentUserId_When_ActivatingUser_Then_FailureResultWithErrorMessageReturned()
        {
            // Given
            var nonExistentUserId = 999;

            // When
            var result = await _userRepository.Activate(nonExistentUserId);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_NonExistentUserId_When_RemovingUser_Then_FailureResultWithErrorMessageReturned()
        {
            // Given
            var nonExistentUserId = 999;

            // When
            var result = await _userRepository.Remove(nonExistentUserId);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }
        */
        #endregion
    }
}
