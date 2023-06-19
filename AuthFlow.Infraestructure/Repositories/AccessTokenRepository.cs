using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Persistence.Data;
using System.Linq.Expressions;

namespace AuthFlow.Infraestructure.Repositories
{
    public class AccessTokenRepository : EntityRepository<AccessToken>, IAccessTokenRepository
    {
        public AccessTokenRepository(AuthFlowDbContext context) : base(context)
        {
        }

        public Task<OperationResult<bool>> ActivateAccessToken(int id)
        {
            return this.Activate(id);
        }

        public Task<OperationResult<int>> CreateAccessToken(AccessToken entity)
        {
            return this.Add(entity);
        }


        public Task<OperationResult<bool>> DeleteAccessToken(int id)
        {
            return this.Remove(id);
        }

        public Task<OperationResult<bool>> DisableAccessToken(int id)
        {
            return this.Deactivate(id);
        }

        public Task<OperationResult<IQueryable<AccessToken>>> GetAccessTokensAll()
        {
            return this.RetrieveAll();
        }

        public Task<OperationResult<IQueryable<AccessToken>>> GetAccessTokensByFilter(Expression<Func<AccessToken, bool>> predicate)
        {
            return this.RetrieveByFilter(predicate);
        }

        public Task<OperationResult<bool>> UpdateAccessToken(AccessToken entity)
        {
            return this.Modify(entity);
        }

        protected override async Task<OperationResult<bool>> ValidateEntity(AccessToken entity, int? updatingUserId = null)
        {
            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.NecesaryData);
            }

            var userByEmail = await base.GetEntitiesByFilter(p => p.UserId == entity.UserId && p.Token == entity.Token  && p.Id != updatingUserId);
            var userExistByEmail = userByEmail.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
