using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using System.Linq.Expressions;

// Namespace for application repository interfaces
namespace AuthFlow.Application.Repositories.Interface
{
    // Interface for the AccessToken repository.
    // This interface defines methods for interacting with the AccessToken data in the repository.
    public interface IAccessTokenRepository
    {
        // Returns all AccessTokens in the repository
        Task<OperationResult<IQueryable<AccessToken>>> GetAccessTokensAll();

        // Returns a subset of AccessTokens based on the provided predicate
        Task<OperationResult<IQueryable<AccessToken>>> GetAccessTokensByFilter(Expression<Func<AccessToken, bool>> predicate);

        // Adds an AccessToken entity to the repository and returns the id of the added AccessToken
        Task<OperationResult<int>> CreateAccessToken(AccessToken entity);

        // Updates an AccessToken in the repository and returns a boolean indicating if the update was successful
        Task<OperationResult<bool>> UpdateAccessToken(AccessToken entity);

        // Deletes an AccessToken with the provided id from the repository and returns a boolean indicating if the deletion was successful
        Task<OperationResult<bool>> DeleteAccessToken(int id);

        // Disables an AccessToken with the provided id and returns a boolean indicating if the operation was successful
        Task<OperationResult<bool>> DisableAccessToken(int id);

        // Activates an AccessToken with the provided id and returns a boolean indicating if the operation was successful
        Task<OperationResult<bool>> ActivateAccessToken(int id);
    }
}
