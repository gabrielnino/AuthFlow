namespace AuthFlow.Test.Infraestructure.Repository.BaseTest
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Infraestructure.Repositories;
    using AuthFlow.Persistence.Data;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using AuthFlow.Application.Use_cases.Interface.Operations;
    using AuthFlow.Application.Repositories.Interface;
    using AuthFlow.Persistence.Data.Interface;

    [TestFixture]
    public class BaseTests
    {
        protected IUsersRepository _userRepository;
        protected AuthFlowDbContext _dbContextMock;
        protected DbContextOptions<AuthFlowDbContext> _options;
        protected Mock<ILogService> _externalLogService;
        protected Mock<IConfiguration> _configuration;
        protected Mock<IConfigurationSection> _configurationSection;
        protected Mock<IOTPServices> _otpService;
        protected IUserDataGenerator _userDataGenerator;
        protected IUserFactory _userFactory;
        protected Mock<IConfigurationSection> _configSectionUsername;
        protected Mock<IConfigurationSection> _configSectionPassword;
        protected Mock<IConfigurationSection> _configSectionEmail;
        protected Mock<IConfigurationSection> _configSectionMasive;
        protected IDataSeeder _dataSeeder;

        [SetUp]
        public void Setup()
        {
            _externalLogService = new Mock<ILogService>();
            _configuration = new Mock<IConfiguration>();
            _otpService = new Mock<IOTPServices>();

            _configurationSection = new Mock<IConfigurationSection>();
            _configurationSection.SetupGet(m => m.Value).Returns("ssnDBVccFUhVvPWQPh7LssnDBVccFUhVvPWQPh7L");
            _configuration.Setup(config => config.GetSection(It.IsAny<string>())).Returns(_configurationSection.Object);
            
            _options = new DbContextOptionsBuilder<AuthFlowDbContext>()
               .UseInMemoryDatabase(databaseName: "testdb")
               .Options;
            _configSectionUsername = new Mock<IConfigurationSection>();
            _configSectionPassword = new Mock<IConfigurationSection>();
            _configSectionEmail = new Mock<IConfigurationSection>();
            _configSectionMasive = new Mock<IConfigurationSection>();

            _configSectionUsername
               .Setup(x => x.Value)
               .Returns("usernameanonymous");
            _configSectionPassword
                .Setup(x => x.Value)
                .Returns("72b28030ce99fa4d0ab678f1d4a4374cc0d7bb676eb4307b0fa105f4c66b644e");
            _configSectionEmail
                .Setup(x => x.Value)
                .Returns("user.anonymous@withoutemail.com");
            _configSectionMasive
                .Setup(x => x.Value)
                .Returns("false");
            _configuration
                .Setup(section => section.GetSection("anonymous:username"))
                .Returns(_configSectionUsername.Object);
            _configuration
                .Setup(section => section.GetSection("anonymous:password"))
                .Returns(_configSectionPassword.Object);
            _configuration
                .Setup(section => section.GetSection("anonymous:email"))
                .Returns(_configSectionEmail.Object);
            _configuration
                .Setup(section => section.GetSection("genesys:isMassive"))
                .Returns(_configSectionMasive.Object);


            _userFactory = new UserFactory();
            _userDataGenerator = new UserDataGenerator(_userFactory, _configuration.Object);
            _dbContextMock =  new AuthFlowDbContext(_options);
            _dataSeeder = new DataSeeder(_dbContextMock, _userDataGenerator, _configuration.Object);
            _userRepository = new UsersRepository(_dbContextMock,
                _externalLogService.Object,
                _configuration.Object,
                _otpService.Object,
                _dataSeeder);
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