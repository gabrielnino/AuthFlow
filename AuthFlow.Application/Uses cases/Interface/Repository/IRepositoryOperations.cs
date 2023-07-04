using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Interfaces;
using System.Linq.Expressions;

// Namespace for application repository interfaces
namespace AuthFlow.Application.Repositories.Interface.Repository
{
    // Interface for generic repository operations.
    // This interface defines common methods for interacting with the data in the repository.
    public interface IRepositoryOperations<T> where T : class, IEntity
    {

        // Returns a subset of entities of type T based on the provided predicate
        Task<OperationResult<IQueryable<T>>> GetAllByFilter(Expression<Func<T, bool>> predicate);

        Task<OperationResult<IQueryable<T>>> GetPageByFilter(int pageNumber, int pageSize, string filter);

        Task<OperationResult<int>> GetCountByFilter(string filter);

        // Adds an entity of type T to the repository and returns the id of the added entity
        Task<OperationResult<int>> Add(T entity);

        // Updates an entity of type T in the repository and returns a boolean indicating if the update was successful
        Task<OperationResult<bool>> Modified(T entity);

        // Deletes an entity with the provided id from the repository and returns a boolean indicating if the deletion was successful
        Task<OperationResult<bool>> Remove(int id);

        // Deactivates an entity with the provided id and returns a boolean indicating if the operation was successful
        Task<OperationResult<bool>> Deactivate(int id);

        // Activates an entity with the provided id and returns a boolean indicating if the operation was successful
        Task<OperationResult<bool>> Activate(int id);
    }
}
