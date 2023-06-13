using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Persistence.Data;
using AuthFlow.Persistence.Repositories;

namespace AuthFlow.Infraestructure.Repositories
{
    public class UsersRepository : Repository<User>, IUserRepository
    {
        public UsersRepository(AuthFlowDbContext context) : base(context)
        {

        }
    }
}