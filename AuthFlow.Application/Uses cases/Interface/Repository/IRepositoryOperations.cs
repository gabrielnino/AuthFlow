using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Interfaces;
using System.Linq.Expressions;

// The namespace AuthFlow.Application.Repositories.Interface.Repository contains the interface definitions 
// for the repository layer in the application. This repository layer handles the interaction between the application 
// and the data source (like a database).
namespace AuthFlow.Application.Repositories.Interface.Repository
{
    // The IRepositoryOperations<T> interface defines the contract for a generic repository, where T is an entity in the domain.
    // It contains methods for the basic CRUD operations as well as other common operations for entities in a repository.
    public interface IRepositoryOperations<T> where T : class, IEntity
    {

        // The GetUserById method returns the entity by Id.
        // It takes a lambda expression that represents a condition the entities should meet.
        Task<OperationResult<T>> GetUserById(int id);

        // The GetAllByFilter method returns all entities of type T that satisfy the provided predicate.
        // It takes a lambda expression that represents a condition the entities should meet.
        Task<OperationResult<IQueryable<T>>> GetAllByFilter(Expression<Func<T, bool>> predicate);

        // The GetPageByFilter method returns a paginated list of entities of type T based on the provided filter string.
        // The pageNumber and pageSize parameters control the pagination.
        Task<OperationResult<IQueryable<T>>> GetPageByFilter(int pageNumber, int pageSize, string filter);

        // The GetCountByFilter method returns the number of entities that satisfy the provided filter string.
        Task<OperationResult<int>> GetCountByFilter(string filter);

        // The Add method adds a new entity to the repository and returns the ID of the added entity.
        Task<OperationResult<int>> Add(T entity);

        // The Modified method updates an existing entity in the repository.
        // It returns a boolean indicating whether the operation was successful.
        Task<OperationResult<bool>> Modified(T entity);

        // The Remove method deletes an entity from the repository.
        // It takes the ID of the entity to be deleted, and returns a boolean indicating whether the operation was successful.
        Task<OperationResult<bool>> Remove(int id);

        // The Deactivate method deactivates an entity in the repository.
        // It takes the ID of the entity to be deactivated, and returns a boolean indicating whether the operation was successful.
        Task<OperationResult<bool>> Deactivate(int id);

        // The Activate method activates a deactivated entity in the repository.
        // It takes the ID of the entity to be activated, and returns a boolean indicating whether the operation was successful.
        Task<OperationResult<bool>> Activate(int id);
    }
}
