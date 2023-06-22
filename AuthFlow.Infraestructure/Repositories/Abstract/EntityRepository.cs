using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Interfaces;
using AuthFlow.Infraestructure;
using AuthFlow.Persistence.Data;
using AuthFlow.Persistence.Repositories;
using System.Linq.Expressions;

// Namespace for infrastructure repositories
namespace AuthFlow.Infraestructure.Repositories.Abstract
{
    // Base class for Entity Repositories, provides common CRUD operations for all entities that extend from IEntity
    public abstract class EntityRepository<T> : Repository<T>, IRepositoryOperations<T> where T : class, IEntity
    {
        // Constructor that takes a AuthFlowDbContext object as a parameter
        public EntityRepository(AuthFlowDbContext context) : base(context)
        {
        }

        // Method to activate a specific entity by its ID.
        public async Task<OperationResult<bool>> Activate(int id)
        {
            try
            {
                // Custom message for validation
                var messageExist = string.Format(Resource.GenericToInactiveNotExist, typeof(T).Name);

                // Validate if the entity with the provided ID exists
                var validationResult = await ValidateExist(id, messageExist);

                // If validation is not successful, return a failure operation result
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }

                // If validation is successful, set the entity as active
                var entity = validationResult.Data;
                entity.Active = true;

                // Update the entity in the database
                var result = await UpdateEntity(entity);

                // Custom success message
                var messageSuccess = string.Format(Resource.SuccessfullyGenericActiveated, typeof(T).Name);

                // Return a success operation result
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch
            {
                // TODO: Handle exception and add log
                // Return a failure operation result with a custom error message
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to add a new entity.
        public async Task<OperationResult<int>> Add(T entity)
        {
            try
            {
                // Validate the entity
                var validationResult = await ValidateEntity(entity);

                // If validation is not successful, return a failure operation result with a custom error message
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<int>.Failure(validationResult.Message);
                }

                // If validation is successful, create the entity in the database
                var result = await CreateEntity(entity);

                // Custom success message
                var messageExist = string.Format(Resource.SuccessfullyGeneric, typeof(T).Name);

                // Return a success operation result
                return OperationResult<int>.Success(result, messageExist);
            }
            catch
            {
                // TODO: Handle exception and add log
                // Return a failure operation result with a custom error message
                return OperationResult<int>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to deactivate a specific entity by its ID.
        public async Task<OperationResult<bool>> Deactivate(int id)
        {
            try
            {
                // Custom message for validation
                var messageExist = string.Format(Resource.UserToInactiveNotExist, typeof(T).Name);

                // Validate if the entity with the provided ID exists
                var validationResult = await ValidateExist(id, messageExist);

                // If validation is not successful, return a failure operation result
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }

                // If validation is successful, set the entity as inactive
                var entity = validationResult.Data;
                entity.Active = false;

                // Update the entity in the database
                var result = await UpdateEntity(entity);

                // Custom success message
                var messageSuccess = string.Format(Resource.SuccessfullyGenericDisabled, typeof(T).Name);

                // Return a success operation result
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch
            {
                // TODO: Handle exception and add log
                // Return a failure operation result with a custom error message
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to modify an existing entity.
        public async Task<OperationResult<bool>> Modify(T entity)
        {
            try
            {
                // Validate the entity
                var validationResult = await ValidateEntity(entity, entity.Id);

                // If validation is not successful, return a failure operation result with a custom error message
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult.Message);
                }

                // If validation is successful, update the entity in the database
                var result = await UpdateEntity(entity);

                // Custom success message
                var messageSuccess = string.Format(Resource.SuccessfullyGenericUpdated, typeof(T).Name);

                // Return a success operation result
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch
            {
                // TODO: Handle exception and add log
                // Return a failure operation result with a custom error message
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to remove a specific entity by its ID.
        public async Task<OperationResult<bool>> Remove(int id)
        {
            try
            {
                // Custom success message for validation
                var messageDeleted = string.Format(Resource.SuccessfullyGenericDeleted, typeof(T).Name);

                // Validate if the entity with the provided ID exists
                var validationResult = await ValidateExist(id, messageDeleted);

                // If validation is not successful, return a failure operation result
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }

                // If validation is successful, delete the entity from the database
                var entity = validationResult.Data;
                bool result = await DeleteEntity(entity);

                // Custom success message
                var messageSuccess = string.Format(Resource.SuccessfullyGenericDeleted, typeof(T).Name);

                // Return a success operation result
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch
            {
                // TODO: Handle exception and add log
                // Return a failure operation result with a custom error message
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to retrieve all entities.
        public async Task<OperationResult<IQueryable<T>>> RetrieveAll()
        {
            try
            {
                // Get all entities from the database
                var result = await GetAll();

                // Custom success message
                var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);

                // Return a success operation result
                return OperationResult<IQueryable<T>>.Success(result, messageSuccessfully);
            }
            catch
            {
                // TODO: Handle exception and add log
                // Return a failure operation result with a custom error message
                return OperationResult<IQueryable<T>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to retrieve entities based on a filter expression.
        public async Task<OperationResult<IQueryable<T>>> RetrieveByFilter(Expression<Func<T, bool>> predicate)
        {
            try
            {
                // Get entities from the database based on the provided filter expression
                var result = await GetEntitiesByFilter(predicate);

                // Custom success message
                var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);

                // Return a success operation result
                return OperationResult<IQueryable<T>>.Success(result, messageSuccessfully);
            }
            catch
            {
                // TODO: Handle exception and add log
                // Return a failure operation result with a custom error message
                return OperationResult<IQueryable<T>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }


        // Abstract method to validate an entity, must be overridden in derived classes
        protected abstract Task<OperationResult<bool>> ValidateEntity(T entity, int? updatingUserId = null);

        // Method to validate if an entity exists based on its ID.
        private async Task<OperationResult<T>> ValidateExist(int id, string message)
        {
            // Validate the provided ID
            if (id == 0)
            {
                return OperationResult<T>.Failure(Resource.NecesaryData);
            }

            // Get the entity from the database based on the provided ID
            var entitiesToValidate = await GetEntitiesByFilter(p => p.Id == id);
            var entity = entitiesToValidate.FirstOrDefault();

            // If the entity does not exist, return a failure operation result with a custom error message
            if (entity is null)
            {
                return OperationResult<T>.Failure(message);
            }

            // If the entity exists, return a success operation result
            return OperationResult<T>.Success(entity, string.Empty);
        }
    }
}