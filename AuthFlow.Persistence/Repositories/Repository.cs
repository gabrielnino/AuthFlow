using AuthFlow.Domain.Interfaces;
using AuthFlow.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

// Namespace for persistence repository implementations
namespace AuthFlow.Persistence.Repositories
{
    // Abstract base class for a generic repository.
    // This class defines methods for interacting with data in the repository using DbContext.
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        // Private variables to hold the DbContext and DbSet<T> instances
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        // Constructor that takes a DbContext object as a parameter
        // This constructor is used to initialize the DbContext and DbSet instances
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
        public async Task<IQueryable<T>> GetAllByFilter(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        // Finds entities of type T that match a given predicate
        public async Task<IQueryable<T>> GetPageByFilter(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            // Calculate the number of entities to skip based on the current page number
            int skip = pageNumber * pageSize;
            return _dbSet.Where(predicate)
                              .OrderBy(p => p.Id) // Assuming that Id is a field you want to sort by
                              .Skip(skip)
                              .Take(pageSize);
        }


        public async Task<int> GetCountFilter(Expression<Func<T, bool>> predicate)
        {
            // Calculate the number of entities to skip based on the current page number
            return _dbSet.Where(predicate).Count();
        }


        // Adds an entity of type T to the database
        // This method sets the entity Id to 0, adds the entity to the DbSet, 
        // and then saves the changes to the database asynchronously.
        public async Task<int> Add(T entity)
        {
            entity.Id = 0;
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        // Modifies an existing entity of type T in the database
        // This method sets the EntityState to Modified, and then saves the changes to the database asynchronously.
        // The method returns a boolean indicating whether the operation was successful.
        public async Task<bool> Modified(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result.Equals(1);
        }

        // Deletes an entity of type T from the database
        // This method removes the entity from the DbSet, and then saves the changes to the database asynchronously.
        // The method returns a boolean indicating whether the operation was successful.
        public async Task<bool> Remove(T entity)
        {
            _dbSet.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result.Equals(1);
        }
    }
}
