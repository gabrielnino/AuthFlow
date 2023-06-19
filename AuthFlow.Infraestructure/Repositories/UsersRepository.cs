using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Persistence.Data;
using System.Linq.Expressions;

namespace AuthFlow.Infraestructure.Repositories
{
    public class UsersRepository : EntityRepository<User>, IUserRepository
    {
        public UsersRepository(AuthFlowDbContext context) : base(context)
        {
        }

        public Task<OperationResult<bool>> ActivateUser(int id)
        {
            return this.Activate(id);
        }

        public Task<OperationResult<int>> CreateUser(User entity)
        {
            return this.Add(entity);
        }

        public Task<OperationResult<bool>> DeleteUser(int id)
        {
            return this.Remove(id);
        }

        public Task<OperationResult<bool>> DisableUser(int id)
        {
            return this.Deactivate(id);
        }

        public Task<OperationResult<IQueryable<User>>> GetUsersAll()
        {
            return this.RetrieveAll();
        }

        public Task<OperationResult<IQueryable<User>>> GetUsersByFilter(Expression<Func<User, bool>> predicate)
        {
            return this.RetrieveByFilter(predicate);
        }

        public Task<OperationResult<bool>> UpdateUser(User entity)
        {
            return this.Modify(entity);
        }

        protected override async Task<OperationResult<bool>> ValidateEntity(User entity, int? updatingUserId = null)
        {
            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.NecesaryData);
            }

            var userByEmail = await base.GetEntitiesByFilter(p => p.Email == entity.Email && p.Id != updatingUserId);
            var userExistByEmail = userByEmail.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            var userByUserName = await base.GetEntitiesByFilter(p => p.Username == entity.Username && p.Id != updatingUserId);
            var userExistByUserName = userByUserName.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredUser);
            }

            return OperationResult<bool>.Success(true);
        }
    }
}