using AuthFlow.Application.Uses_cases.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Cryptography;
using System.Text;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class BaseTests
    {
        public UsersRepository _userRepository;
        public AuthFlowDbContext _dbContextMock;
        public DbContextOptions<AuthFlowDbContext> _options;
        public Mock<IExternalLogService> _externalLogService;

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


        protected static string GetMaximumLength(int maximumLength, string value)
        {
            if (maximumLength > 0)
            {
                if (value.Length < maximumLength)
                {
                    value = value.PadRight(maximumLength +1, '_');
                }
            }

            return value;
        }

        protected static string GetMinimumLength(int minimumLength, string value)
        {
            if (minimumLength > 0)
            {
                if (value.Length > minimumLength)
                {
                    value = value.Substring(0, minimumLength - 1);
                }
            }

            return value;
        }


        protected static User GetUser(string name = "doe", int minimumLength = 0, int maximumLength = 0)
        {

            var userName = $"john.{name}";
            userName = GetValueModified(minimumLength, maximumLength, userName);

            var email = $"john.{name}@example.com";
            email = GetValueModified(minimumLength, maximumLength, email);

            var password = "password";
            password = GetValueModified(minimumLength, maximumLength, password);

            return new User
            {
                Username = userName,
                Email = email,
                Password = password
            };
        }

        protected static string GetValueModified(int minimumLength, int maximumLength, string userName)
        {
            userName = GetMaximumLength(maximumLength, userName);
            userName = GetMinimumLength(minimumLength, userName);
            return userName;
        }

        protected static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using SHA256 sha256Hash = SHA256.Create();
            // ComputeHash - returns byte array  
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            var builder = new StringBuilder();
            foreach (byte v in bytes)
            {
                builder.Append(v.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}