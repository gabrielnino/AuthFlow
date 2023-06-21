using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Infraestructure;
using AuthFlow.Persistence.Data;
using System.Linq.Expressions;

// Namespace for infrastructure repositories
namespace AuthFlow.Infrastructure.Repositories
{
    // Implementation of the IAccessTokenRepository interface for managing AccessTokens
    public class AccessTokenRepository : EntityRepository<AccessToken>, IAccessTokenRepository
    {
        // Constructor that takes a AuthFlowDbContext object as a parameter
        public AccessTokenRepository(AuthFlowDbContext context) : base(context)
        {
        }

        // Activates an AccessToken by id
        public Task<OperationResult<bool>> ActivateAccessToken(int id)
        {
            return this.Activate(id);
        }

        // Creates a new AccessToken
        public Task<OperationResult<int>> CreateAccessToken(AccessToken entity)
        {
            return this.Add(entity);
        }

        // Deletes an AccessToken by id
        public Task<OperationResult<bool>> DeleteAccessToken(int id)
        {
            return this.Remove(id);
        }

        // Deactivates an AccessToken by id
        public Task<OperationResult<bool>> DisableAccessToken(int id)
        {
            return this.Deactivate(id);
        }

        // Retrieves all AccessTokens
        public Task<OperationResult<IQueryable<AccessToken>>> GetAccessTokensAll()
        {
            return this.RetrieveAll();
        }

        // Retrieves AccessTokens by filter
        public Task<OperationResult<IQueryable<AccessToken>>> GetAccessTokensByFilter(Expression<Func<AccessToken, bool>> predicate)
        {
            return this.RetrieveByFilter(predicate);
        }

        // Updates an existing AccessToken
        public Task<OperationResult<bool>> UpdateAccessToken(AccessToken entity)
        {
            return this.Modify(entity);
        }

        // Validates an AccessToken entity, checking if it is not null and if its UserId and Token
        // are not duplicated (excluding the AccessToken with the provided updatingUserId if it is not null).
        // Returns an OperationResult indicating the result of the validation.
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
