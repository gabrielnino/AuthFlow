using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Interfaces;
using System.Linq.Expressions;

namespace AuthFlow.Application.Repositories.Interface
{
    public interface IRepositoryOperations<T> where T : class, IEntity
    {
        // Returns all entities of type T in the repository
        Task<OperationResult<IQueryable<T>>> RetrieveAll();
        //// Returns a subset of entities of type T based on the predicate specified
        Task<OperationResult<IQueryable<T>>> RetrieveByFilter(Expression<Func<T, bool>> predicate);

        // Adds an entity of type T to the repository
        Task<OperationResult<int>> Add(T entity);

        // Deletes an entity of type T from the repository
        Task<OperationResult<bool>> Modify(T entity);

        //// Updates an entity of type T in the repository
        Task<OperationResult<bool>> Remove(int id);

        //// Deletes an entity of type T from the repository
        Task<OperationResult<bool>> Deactivate(int id);

        Task<OperationResult<bool>> Activate(int id);
    }
}
