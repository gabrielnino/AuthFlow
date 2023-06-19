using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;
using AuthFlow.Domain.Interfaces;
using System.Linq.Expressions;

namespace AuthFlow.Application.Repositories.Interface
{
    public interface IEntityRepository<T> where T : class, IEntity
    {
        // Returns all entities of type T in the repository
        Task<OperationResult<IQueryable<T>>> GetEntitiesAll();
        // Returns a subset of entities of type T based on the predicate specified
        Task<OperationResult<IQueryable<T>>> GetEntitiesByFilter(Expression<Func<T, bool>> predicate);

        // Adds an entity of type T to the repository
        Task<OperationResult<int>> CreateEntity(T entity);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> UpdateEntity(T entity);

        // Updates an entity of type T in the repository
        Task<OperationResult<bool>> DeleteEntity(int id);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> DisableEntity(int id);

        Task<OperationResult<bool>> ActivateEntity(int id);
    }
}
