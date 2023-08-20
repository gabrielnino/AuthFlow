
namespace AuthFlow.Persistence.Data
{
    using AuthFlow.Persistence.Data.Interface;
    using Microsoft.Extensions.Configuration;


    public class DataSeeder : IDataSeeder
    {
        private readonly IAuthFlowDbContext _dbContext;
        private readonly IUserDataGenerator _userDataGenerator;
        private readonly bool _isMassiveData;

        private const int DEFAULT_PAGE_SIZE = 10000;

        public DataSeeder(IAuthFlowDbContext dbContext, IUserDataGenerator userDataGenerator, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userDataGenerator = userDataGenerator ?? throw new ArgumentNullException(nameof(userDataGenerator));

            if (bool.TryParse(configuration["genesys:massive"], out bool massiveSetting))
            {
                _isMassiveData = massiveSetting;
            }
        }

        public void SeedData()
        {
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
