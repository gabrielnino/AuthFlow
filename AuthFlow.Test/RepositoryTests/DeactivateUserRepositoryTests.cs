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
    public class DeactivateUserRepositoryTests : UtilTests
    {
        private const string success = "User was disabled successfully.";
        private const string userTryingInactiveDoesNotExist = "The user you are trying to inactive does not exist.";

        private UsersRepository _userRepository;
        private AuthFlowDbContext _dbContextMock;
        private DbContextOptions<AuthFlowDbContext> _options;
        private Mock<IExternalLogService> _externalLogService;

        [SetUp]
        public void Setup()
        {
            _externalLogService = new Mock<IExternalLogService>();
            _options = new DbContextOptionsBuilder<AuthFlowDbContext>()
               .UseInMemoryDatabase(databaseName: "testdb")
               .Options;
            _dbContextMock =  new AuthFlowDbContext(_options);
            _userRepository = new UsersRepository(_dbContextMock, _externalLogService.Object);
        }

        [Test]
        public async Task Given_user_When_DeactivateUser_Then_SuccessResultWithTrue()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Deactivate(user.Id);

            var userRepo = await _userRepository.GetAllByFilter(u => u.Id.Equals(user.Id));
            var userFound = userRepo.Data.FirstOrDefault();

            // Then
            result?.Message.Should().Be(success);
            userFound.Active.Should().BeFalse();
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Test]
        public async Task Given_user_When_DeactivateUser_Then_FailedResultWithFalse()
        {
            // Given
            var name = Guid.NewGuid().ToString();
            User user = GetUser(name);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;

            // When
            var result = await _userRepository.Deactivate(999999);

            // Then
            result?.Message.Should().Be(userTryingInactiveDoesNotExist);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeFalse();
        }
    }
}