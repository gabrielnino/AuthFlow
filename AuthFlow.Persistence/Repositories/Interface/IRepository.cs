using AuthFlow.Domain.Interfaces;
using System.Linq.Expressions;

namespace AuthFlow.Persistence.Repositories.Interface
{
    public interface IRepository<T> where T : class, IEntity
    {
        // Returns all entities of type T in the repository
        Task<IQueryable<T>> GetAll();
        // Returns a subset of entities of type T based on the predicate specified
        Task<IQueryable<T>> GetEntitiesByFilter(Expression<Func<T, bool>> predicate);

        // Adds an entity of type T to the repository
        Task<int> CreateEntity(T entity);

        // Deletes an entity of type T from the repository
        Task<bool> UpdateEntity(T entity);

        // Updates an entity of type T in the repository
        Task<bool> DeleteEntity(T entity);
    }
}
