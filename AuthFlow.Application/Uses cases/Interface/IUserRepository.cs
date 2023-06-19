using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using System.Linq.Expressions;

namespace AuthFlow.Application.Repositories.Interface
{
    public interface IUserRepository
    {
        // Returns all entities of type T in the repository
        Task<OperationResult<IQueryable<User>>> GetUsersAll();
        // Returns a subset of entities of type T based on the predicate specified
        Task<OperationResult<IQueryable<User>>> GetUsersByFilter(Expression<Func<User, bool>> predicate);

        // Adds an entity of type T to the repository
        Task<OperationResult<int>> CreateUser(User entity);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> UpdateUser(User entity);

        // Updates an entity of type T in the repository
        Task<OperationResult<bool>> DeleteUser(int id);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> DisableUser(int id);

        Task<OperationResult<bool>> ActivateUser(int id);
    }
}
