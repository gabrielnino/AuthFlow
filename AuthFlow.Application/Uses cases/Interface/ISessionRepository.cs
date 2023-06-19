using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using System.Linq.Expressions;

namespace AuthFlow.Application.Repositories.Interface
{
    public interface ISessionRepository
    {
        // Returns all entities of type T in the repository
        Task<OperationResult<IQueryable<Session>>> GetSessionsAll();
        // Returns a subset of entities of type T based on the predicate specified
        Task<OperationResult<IQueryable<Session>>> GetSessionsByFilter(Expression<Func<Session, bool>> predicate);

        // Adds an entity of type T to the repository
        Task<OperationResult<int>> CreateSession(Session entity);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> UpdateSession(Session entity);

        // Updates an entity of type T in the repository
        Task<OperationResult<bool>> DeleteSession(int id);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> DisableSession(int id);

        Task<OperationResult<bool>> ActivateSession(int id);
    }
}
