/// The namespace AuthFlow.Application.Repositories.Interface.Repository contains the interface definitions 
/// for the repository layer in the application. This repository layer handles the interaction between the application 
/// and the data source (like a database).
namespace AuthFlow.Application.Repositories.Interface.Repository
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Domain.Interfaces;
    using System.Linq.Expressions;

    /// <summary>
    /// The IRepositoryOperations<T> interface defines the contract for a generic repository, where T is an entity in the domain.
    /// It contains methods for the basic CRUD operations as well as other common operations for entities in a repository.
    /// </summary>
    /// <typeparam name="T">Is an IEntity class</typeparam>
    public interface IRepositoryOperations<T> where T : class, IEntity
    {

        /// <summary>
        /// The GetUserById method returns the entity by Id.
        /// It takes a lambda expression that represents a condition the entities should meet.
        /// </summary>
        /// <param name="id">The Id</param>
        /// <returns>The result of the operation</returns>
        Task<OperationResult<T>> GetUserById(int id);

        /// <summary>
        /// The GetAllByFilter method returns all entities of type T that satisfy the provided predicate.
        /// It takes a lambda expression that represents a condition the entities should meet.
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>The result of the operation</returns>
        Task<OperationResult<IQueryable<T>>> GetAllByFilter(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// The GetPageByFilter method returns a paginated list of entities of type T based on the provided filter string.
        /// The pageNumber and pageSize parameters control the pagination.
        /// </summary>
        /// <param name="pageNumber">The page number</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="filter">The filter</param>
        /// <returns>The result of the opertion</returns>
        Task<OperationResult<IQueryable<T>>> GetPageByFilter(int pageNumber, int pageSize, string filter);

        /// <summary>
        /// The GetCountByFilter method returns the number of entities that satisfy the provided filter string.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <returns>The result of the operation</returns>
        Task<OperationResult<int>> GetCountByFilter(string filter);

        /// <summary>
        /// The Add method adds a new entity to the repository and returns the ID of the added entity.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The result of the operation</returns>
        Task<OperationResult<int>> Add(T entity);

        /// <summary>
        /// The Modified method updates an existing entity in the repository.
        /// It returns a boolean indicating whether the operation was successful.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The result of the operation</returns>
        Task<OperationResult<bool>> Modified(T entity);

        /// <summary>
        /// The Remove method deletes an entity from the repository.
        /// It takes the ID of the entity to be deleted, and returns a boolean indicating whether the operation was successful. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OperationResult<bool>> Remove(int id);

        /// <summary>
        /// The Deactivate method deactivates an entity in the repository.
        /// It takes the ID of the entity to be deactivated, and returns a boolean indicating whether the operation was successful.
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The result of the operation</returns>
        Task<OperationResult<bool>> Deactivate(int id);

        /// <summary>
        /// The Activate method activates a deactivated entity in the repository.
        /// It takes the ID of the entity to be activated, and returns a boolean indicating whether the operation was successful.
        /// </summary>
        /// <param name="id">The Id</param>
        /// <returns>The result of the operation.</returns>
        Task<OperationResult<bool>> Activate(int id);

        /// <summary>
        /// Get the count of the items of by filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The result of the operation.</returns>
        Task<OperationResult<int>> GetCountFilter(string filter);
    }
}
