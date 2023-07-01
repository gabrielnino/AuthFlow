using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Persistence.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class AddUsersRepositoryTests
    {


        private const string success = "User was created successfully.";
        private const string necessaryData = "Necessary data was not provided.";
        private UsersRepository _userRepository;
        private AuthFlowDbContext _dbContextMock;
        private DbContextOptions<AuthFlowDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<AuthFlowDbContext>()
               .UseInMemoryDatabase(databaseName: "testdb")
               .Options;
            _dbContextMock =  new AuthFlowDbContext(_options);
            _userRepository = new UsersRepository(_dbContextMock);
        }

        [Test]
        public async Task Given_user_When_AddingUser_Then_SuccessResultWithIdReturned()
        {
            // Given
            User user = GetUser("carson");

            // When
            var result = await _userRepository.Add(user);

            // Then
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeGreaterThan(0);
            result?.Message.Equals(success).Should().BeTrue();
        }

        [Test]
        public async Task Given_user_null_When_AddingUser_Then_FailedResultWithIdReturned()
        {
            // Given
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            User user = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // When
            var result = await _userRepository.Add(user);

            // Then
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().BeGreaterThanOrEqualTo(0);
            result.Message.Equals(necessaryData).Should().BeTrue();    
        }


        private static User GetUser(string name = "doe", int minimumLength = 0, int maximumLength = 0)
        {

            string userName = $"john.{name}";
            userName = GetMaximumLength(maximumLength, userName);
            userName = GetMinimumLength(minimumLength, userName);

            string lastname = $"john.{name}@example.com";
            lastname = GetMaximumLength(maximumLength, lastname);
            lastname = GetMinimumLength(minimumLength, lastname);

            string password = "password";
            password = GetMaximumLength(maximumLength, password);
            password = GetMinimumLength(minimumLength, password);

            return new User
            {
                Username = userName,
                Email = lastname,
                Password = password
            };
        }

        private static string GetMaximumLength(int maximumLength, string value)
        {
            if (maximumLength > 0)
            {
                if (value.Length < maximumLength)
                {
                    value = value.PadRight(maximumLength +1,'_');
                }
            }

            return value;
        }

        private static string GetMinimumLength(int minimumLength, string value)
        {
            if (minimumLength > 0)
            {
                if (value.Length > minimumLength)
                {
                    value = value.Substring(0, value.Length - minimumLength);
                }
            }

            return value;
        }
    }
}