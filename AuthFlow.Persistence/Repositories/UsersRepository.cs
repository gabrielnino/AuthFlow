using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain;
using AuthFlow.Persistence.Data;

namespace AuthFlow.Persistence.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(AuthFlowDbContext context) : base(context)
        {
        }
    }
}