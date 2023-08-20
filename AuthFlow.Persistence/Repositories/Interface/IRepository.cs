// Namespace for persistence repository interfaces
namespace AuthFlow.Persistence.Repositories.Interface
{
    using AuthFlow.Domain.Interfaces;
    using System.Linq.Expressions;
    using System.Threading.Tasks; // Assuming that the 'Task' type is from System.Threading.Tasks namespace

    // Interface for generic repository operations.
    // This interface defines common methods for interacting with the data in the repository.
    public interface IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Returns a subset of entities of type T based on the provided predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of elements satisfying the condition.</returns>
        Task<IQueryable<T>> GetAllByFilter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns a subset of entities of type T based on the provided predicate and pagination parameters.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="pageNumber">The page number starting from 1.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of elements satisfying the condition and limited by pagination parameters.</returns>
        Task<IQueryable<T>> GetPageByFilter(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);

        /// <summary>
        /// Returns the count of entities of type T based on the provided predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the count of entities satisfying the condition.</returns>
        Task<int> GetCountFilter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds an entity of type T to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the id of the added entity.</returns>
        Task<int> Add(T entity);

        /// <summary>
        /// Updates an entity of type T in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the update was successful.</returns>
        Task<bool> Modified(T entity);

        /// <summary>
        /// Deletes an entity of type T from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the deletion was successful.</returns>
        Task<bool> Remove(T entity);
    }
}
