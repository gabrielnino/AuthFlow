using AuthFlow.Domain.Interfaces;
using AuthFlow.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthFlow.Persistence.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        // Constructor that takes a DbContext object as a parameter
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Returns all entities of type T in the database
        public async Task<IQueryable<T>> GetAll()
        {
            return _dbSet;
        }

        // Finds entities of type T that match a given predicate
        public async Task<IQueryable<T>> GetEntitiesByFilter(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        // Adds an entity of type T to the database
        public async Task<int> CreateEntity(T entity)
        {
            entity.Id = 0;
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        // Modifies an existing entity of type T in the database
        public async Task<bool> UpdateEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result.Equals(1);
        }

        // Deletes an entity of type T from the database
        public async Task<bool> DeleteEntity(T entity)
        {
            _dbSet.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result.Equals(1);
        }
    }
}
