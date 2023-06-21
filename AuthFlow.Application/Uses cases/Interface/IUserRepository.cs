using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using System.Linq.Expressions;

// Namespace for application repository interfaces
namespace AuthFlow.Application.Repositories.Interface
{
    // Interface for the User repository.
    // This interface defines methods for interacting with the User data in the repository.
    public interface IUserRepository
    {
        // Returns all Users in the repository
        Task<OperationResult<IQueryable<User>>> GetUsersAll();

        // Returns a subset of Users based on the provided predicate
        Task<OperationResult<IQueryable<User>>> GetUsersByFilter(Expression<Func<User, bool>> predicate);

        // Adds a User entity to the repository and returns the id of the added User
        Task<OperationResult<int>> CreateUser(User entity);

        // Updates a User in the repository and returns a boolean indicating if the update was successful
        Task<OperationResult<bool>> UpdateUser(User entity);

        // Deletes a User with the provided id from the repository and returns a boolean indicating if the deletion was successful
        Task<OperationResult<bool>> DeleteUser(int id);

        // Disables a User with the provided id and returns a boolean indicating if the operation was successful
        Task<OperationResult<bool>> DisableUser(int id);

        // Activates a User with the provided id and returns a boolean indicating if the operation was successful
        Task<OperationResult<bool>> ActivateUser(int id);
    }
}
