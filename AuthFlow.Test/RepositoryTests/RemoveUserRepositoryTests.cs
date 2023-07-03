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
    public class RemoveUserRepositoryTests : UtilTests
    {
        private const string success = "User was deleted successfully.";
        private const string userTryingInDeleteDoesNotExist = "The User you are trying to delete does not exist.";

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