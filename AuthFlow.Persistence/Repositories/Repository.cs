using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Interfaces;
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
            try
            {
                return _dbSet;
            }
            catch (Exception ex)
            {
                // Add logs
                throw;
            }
        }

        // Finds entities of type T that match a given predicate
        public async Task<IQueryable<T>> GetEntitiesByFilter(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _dbSet.Where(predicate);
            }
            catch (Exception ex)
            {
                // Add logs
                throw;
            }
        }

        // Adds an entity of type T to the database
        public async Task<int> CreateEntity(T entity)
        {
            try
            {
                entity.Id = 0;
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception ex)
            {
                // Add logs
                throw;
            }
        }

        // Modifies an existing entity of type T in the database
        public async Task<bool> UpdateEntity(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();
                return result.Equals(1);
            }
            catch (Exception ex)
            {
                // Add logs
                throw;
            }
        }

        // Deletes an entity of type T from the database
        public async Task<bool> DeleteEntity(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result.Equals(1);
            }
            catch (Exception ex)
            {
                // Add logs
                throw;
            }
        }
    }
}
