using AuthFlow.Domain.Interfaces;
using System.Linq.Expressions;

// Namespace for persistence repository interfaces
namespace AuthFlow.Persistence.Repositories.Interface
{
    // Interface for generic repository operations.
    // This interface defines common methods for interacting with the data in the repository.
    public interface IRepository<T> where T : class, IEntity
    {
        // Returns all entities of type T in the repository
        Task<IQueryable<T>> GetAll();

        // Returns a subset of entities of type T based on the provided predicate
        Task<IQueryable<T>> GetAllByFilter(Expression<Func<T, bool>> predicate);

        // Adds an entity of type T to the repository and returns the id of the added entity
        Task<int> Add(T entity);

        // Updates an entity of type T in the repository and returns a boolean indicating if the update was successful
        Task<bool> Modified(T entity);

        // Deletes an entity of type T from the repository and returns a boolean indicating if the deletion was successful
        Task<bool> Remove(T entity);
    }
}
