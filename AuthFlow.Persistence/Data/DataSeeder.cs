namespace AuthFlow.Persistence.Data
{
    using AuthFlow.Persistence.Data.Interface;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides functionality to seed initial data into the database.
    /// </summary>
    public class DataSeeder : IDataSeeder
    {
        private readonly IAuthFlowDbContext _dbContext;
        private readonly IUserDataGenerator _userDataGenerator;
        private readonly bool _isMassiveData;

        /// <summary>
        /// The default page size for data seeding operations.
        /// </summary>
        private const int DEFAULT_PAGE_SIZE = 10000;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeeder"/> class.
        /// </summary>
        /// <param name="dbContext">The database context for data seeding.</param>
        /// <param name="userDataGenerator">The user data generator.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown when userDataGenerator is null.</exception>
        public DataSeeder(IAuthFlowDbContext dbContext, IUserDataGenerator userDataGenerator, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userDataGenerator = userDataGenerator ?? throw new ArgumentNullException(nameof(userDataGenerator));

            if (bool.TryParse(configuration["genesys:massive"], out bool massiveSetting))
            {
                _isMassiveData = massiveSetting;
            }
        }

        /// <summary>
        /// Seeds data into the database.
        /// </summary>
        public void SeedData()
        {
            // Check if users already exist, if not, seed users.
            if (!_dbContext.Users.Any())
            {
                var users = _isMassiveData ? _userDataGenerator.GenerateMassiveUserList() : _userDataGenerator.GetUsers();
                var countPage = (int)Math.Ceiling((double)users.Count() / DEFAULT_PAGE_SIZE);
                for (int pageNumber = 0; pageNumber <= countPage; pageNumber++)
                {
                    int skip = pageNumber * DEFAULT_PAGE_SIZE;
                    var pageUsers = users.Skip(skip)
                        .Take(DEFAULT_PAGE_SIZE);
                    _dbContext.Users.AddRange(pageUsers);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
