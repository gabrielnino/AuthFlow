using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using System.Linq.Expressions;

namespace AuthFlow.Application.Repositories.Interface
{
    public interface IAccessTokenRepository
    {
        // Returns all entities of type T in the repository
        Task<OperationResult<IQueryable<AccessToken>>> GetAccessTokensAll();
        // Returns a subset of entities of type T based on the predicate specified
        Task<OperationResult<IQueryable<AccessToken>>> GetAccessTokensByFilter(Expression<Func<AccessToken, bool>> predicate);

        // Adds an entity of type T to the repository
        Task<OperationResult<int>> CreateAccessToken(AccessToken entity);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> UpdateAccessToken(AccessToken entity);

        // Updates an entity of type T in the repository
        Task<OperationResult<bool>> DeleteAccessToken(int id);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> DisableAccessToken(int id);

        Task<OperationResult<bool>> ActivateAccessToken(int id);
    }
}
