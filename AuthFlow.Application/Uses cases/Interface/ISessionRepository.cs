using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using System.Linq.Expressions;

// Namespace for application repository interfaces
namespace AuthFlow.Application.Repositories.Interface
{
    // Interface for the Session repository.
    // This interface defines methods for interacting with the Session data in the repository.
    public interface ISessionRepository
    {
        // Returns all Sessions in the repository
        Task<OperationResult<IQueryable<Session>>> GetSessionsAll();

        // Returns a subset of Sessions based on the provided predicate
        Task<OperationResult<IQueryable<Session>>> GetSessionsByFilter(Expression<Func<Session, bool>> predicate);

        // Adds a Session entity to the repository and returns the id of the added Session
        Task<OperationResult<int>> CreateSession(Session entity);

        // Updates a Session in the repository and returns a boolean indicating if the update was successful
        Task<OperationResult<bool>> UpdateSession(Session entity);

        // Deletes a Session with the provided id from the repository and returns a boolean indicating if the deletion was successful
        Task<OperationResult<bool>> DeleteSession(int id);

        // Disables a Session with the provided id and returns a boolean indicating if the operation was successful
        Task<OperationResult<bool>> DisableSession(int id);

        // Activates a Session with the provided id and returns a boolean indicating if the operation was successful
        Task<OperationResult<bool>> ActivateSession(int id);
    }
}
