using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface.Repository;
using AuthFlow.Application.Uses_cases.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Domain.Interfaces;
using AuthFlow.Persistence.Data;
using AuthFlow.Persistence.Repositories;
using System.Linq.Expressions;

// Namespace for infrastructure repositories
namespace AuthFlow.Infraestructure.Repositories.Abstract
{
    // Base class for Entity Repositories, provides common CRUD operations for all entities that extend from IEntity
    public abstract class EntityRepository<T> : Repository<T>, IRepositoryOperations<T> where T : class, IEntity
    {
        protected readonly IExternalLogService _externalLogService;
        // Constructor that takes a AuthFlowDbContext object as a parameter
        public EntityRepository(AuthFlowDbContext context, IExternalLogService externalLogService) : base(context)
        {
            _externalLogService = externalLogService;
        }

        // Method to add a new entity.
        public new async Task<OperationResult<int>> Add(T entity)
        {
            try
            {
                var hasEntity = await HasEntity(entity);
                if (!hasEntity.IsSuccessful)
                {
                    return OperationResult<int>.Failure(hasEntity.Message);
                }

                // Validate the entity
                var validationResult = await AddEntity(entity);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<int>.Failure(validationResult?.Message);
                }

                // If validation is successful, add the entity to the database
                var addedEntityResult = await base.Add(validationResult.Data);

                // Create a success message and return the success result
                var successMessage = string.Format(Resource.SuccessfullyGeneric, typeof(T).Name);
                return OperationResult<int>.Success(addedEntityResult, successMessage);
            }
            catch(Exception ex)
            {
                var log = GetLogError(ex, entity, OperationExecute.Add);
                await _externalLogService.CreateLog(log);
                return OperationResult<int>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to modify an existing entity.
        public new async Task<OperationResult<bool>> Modified(T entity)
        {
            try
            {
                var hasEntity = await HasEntity(entity);
                if (!hasEntity.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(hasEntity.Message);
                }

                var resultExist = await ValidateExist(entity.Id);
                if (!resultExist.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(resultExist.Message);
                }

                var resultModifyEntity = await ModifyEntity(entity, resultExist.Data);
                if (!resultModifyEntity.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(resultModifyEntity.Message);
                }

                // If validation is successful, update the entity in the database
                var updateResult = await base.Modified(resultModifyEntity.Data);

                // Custom success message
                var messageSuccess = string.Format(Resource.SuccessfullyGenericUpdated, typeof(T).Name);

                // Return a success operation result
                return OperationResult<bool>.Success(updateResult, messageSuccess);

            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, entity, OperationExecute.Modified);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to activate a specific entity by its ID.
        public async Task<OperationResult<bool>> Activate(int id)
        {
            try
            {
                // Validate if the entity with the provided ID exists
                var validationResult = await ValidateExist(id);

                // If validation is not successful, return a failure operation result
                if (!validationResult.IsSuccessful)
                {
                    var messageExist = string.Format(Resource.GenericToActiveNotExist, typeof(T).Name);
                    return OperationResult<bool>.Failure(messageExist);
                }

                // If validation is successful, set the entity as active
                var entity = validationResult.Data;
                entity.Active = true;

                // Update the entity in the database
                var result = await base.Modified(entity);

                // Custom success message
                var messageSuccess = string.Format(Resource.SuccessfullyGenericActiveated, typeof(T).Name);

                // Return a success operation result
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, id, OperationExecute.Activate);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }



        // Method to deactivate a specific entity by its ID.
        public async Task<OperationResult<bool>> Deactivate(int id)
        {
            try
            {
                // Validate if the entity with the provided ID exists
                var validationResult = await ValidateExist(id);

                // If validation is not successful, return a failure operation result
                if (!validationResult.IsSuccessful)
                {
                    var messageExist = string.Format(Resource.UserToInactiveNotExist, typeof(T).Name);
                    return OperationResult<bool>.Failure(messageExist);
                }

                // If validation is successful, set the entity as inactive
                var entity = validationResult.Data;
                entity.Active = false;

                // Update the entity in the database
                var result = await base.Modified(entity);

                // Custom success message
                var messageSuccess = string.Format(Resource.SuccessfullyGenericDisabled, typeof(T).Name);

                // Return a success operation result
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, id, OperationExecute.Deactivate);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to remove a specific entity by its ID.
        public async Task<OperationResult<bool>> Remove(int id)
        {
            try
            {
                // Validate if the entity with the provided ID exists
                var validationResult = await ValidateExist(id);

                // If validation is not successful, return a failure operation result
                if (!validationResult.IsSuccessful)
                {
                    var messageExist = string.Format(Resource.GenericToDeleteNotExist, typeof(T).Name);
                    return OperationResult<bool>.Failure(messageExist);
                }

                // If validation is successful, delete the entity from the database
                var entity = validationResult.Data;
                bool result = await Remove(entity);

                // Custom success message
                var messageSuccess = string.Format(Resource.SuccessfullyGenericDeleted, typeof(T).Name);

                // Return a success operation result
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, id, OperationExecute.Remove);
                await _externalLogService.CreateLog(log);
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }


        // Method to retrieve entities based on a filter expression.
        public new async Task<OperationResult<IQueryable<T>>> GetAllByFilter(Expression<Func<T, bool>> predicate)
        {
            try
            {
                // Get entities from the database based on the provided filter expression
                var result = await base.GetAllByFilter(predicate);

                // Custom success message
                var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);

                // Return a success operation result
                return OperationResult<IQueryable<T>>.Success(result, messageSuccessfully);
            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetAllByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<IQueryable<T>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to retrieve entities based on a filter expression.
        public new async Task<OperationResult<IQueryable<T>>> GetPageByFilter(int pageNumber, int pageSize, string filter)
        {
            try
            {
                // Get entities from the database based on the provided filter expression
                var predicate = GetPredicate(filter);
                var result =  await base.GetPageByFilter(predicate, pageNumber, pageSize);

                // Custom success message
                var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);

                // Return a success operation result
                return OperationResult<IQueryable<T>>.Success(result, messageSuccessfully);

            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<IQueryable<T>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public new async Task<OperationResult<int>> GetCountByFilter(string filter)
        {
            try
            {
                // Get entities from the database based on the provided filter expression
                var predicate = GetPredicate(filter);
                var result = await base.GetCountFilter(predicate);

                // Custom success message
                var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);

                // Return a success operation result
                return OperationResult<int>.Success(result, messageSuccessfully);

            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetCountByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<int>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        // Method to retrieve entities based on a filter expression.
        public new async Task<OperationResult<int>> GetCountFilter(string filter)
        {
            try
            {
                // Get entities from the database based on the provided filter expression
                var predicate = GetPredicate(filter);
                var result = await base.GetCountFilter(predicate);

                // Custom success message
                var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);

                // Return a success operation result
                return OperationResult<int>.Success(result, messageSuccessfully);

            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<int>.Failure(Resource.FailedOccurredDataLayer);
            }
        }


        internal abstract Expression<Func<T, bool>> GetPredicate(string filter);

        protected static Log GetLogError(Exception ex, object entity, OperationExecute operation)
        {
            var message = $"Error Message: {ex.Message}  StackTrace: {ex.StackTrace}";
            var log = Log.Error(message, entity, operation);
            return log;
        }

        // Abstract method to validate an entity, must be overridden in derived classes
        internal abstract Task<OperationResult<T>> AddEntity(T entity);

        internal virtual async Task<OperationResult<T>> ModifyEntity(T entityModified, T entityUnmodified)
        {
            // Custom success message
            var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);
            return OperationResult<T>.Success(entityModified, messageSuccessfully);
        }

        // If the entity is null, return a failure operation result with a custom error message
        private static async Task<OperationResult<T>> HasEntity(T entity)
        {
            if (entity is null)
            {
                return OperationResult<T>.Failure(Resource.FailedNecesaryData);
            }
            return OperationResult<T>.Success(entity,Resource.GlobalOkMessage);
        }


        // Method to validate if an entity exists based on its ID.
        private async Task<OperationResult<T>> ValidateExist(int id)
        {
            // Validate the provided ID
            if (id.Equals(0))
            {
                return OperationResult<T>.Failure(Resource.FailedNecesaryData);
            }

            // Get the existing user from the repository
            var entityRepo = await base.GetAllByFilter(e => e.Id.Equals(id));
            var entityUnmodified = entityRepo?.FirstOrDefault();
            var hasEntity = entityUnmodified is not null;
            if (!hasEntity)
            {
                return OperationResult<T>.Failure(Resource.FailedNecesaryData);
            }

            // If the entity exists, return a success operation result
            return OperationResult<T>.Success(entityUnmodified, Resource.GlobalOkMessage);
        }
    }
}