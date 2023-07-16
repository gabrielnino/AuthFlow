using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class BaseTests
    {
        protected UsersRepository _userRepository;
        protected AuthFlowDbContext _dbContextMock;
        protected DbContextOptions<AuthFlowDbContext> _options;
        protected Mock<ILogService> _externalLogService;
        protected Mock<IConfiguration> _configuration;
        protected Mock<IConfigurationSection> _configurationSection;

        [SetUp]
        public void Setup()
        {
            _externalLogService = new Mock<ILogService>();
            _configuration = new Mock<IConfiguration>();
            _configurationSection = new Mock<IConfigurationSection>();
            _configurationSection.SetupGet(m => m.Value).Returns("ssnDBVccFUhVvPWQPh7LssnDBVccFUhVvPWQPh7L");
            _configuration.Setup(config => config.GetSection(It.IsAny<string>())).Returns(_configurationSection.Object);
            
            _options = new DbContextOptionsBuilder<AuthFlowDbContext>()
               .UseInMemoryDatabase(databaseName: "testdb")
               .Options;
            _dbContextMock =  new AuthFlowDbContext(_options);
            _userRepository = new UsersRepository(_dbContextMock, _externalLogService.Object, _configuration.Object);
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

        protected static User GetUser(string userName, string email)
        {

            var password = "password";

            return new User
            {
                Username = userName,
                Email = email,
                Password = password
            };
        }

        protected static User GetUser(string userName, string email, string password)
        {

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