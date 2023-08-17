// Namespace for infrastructure repositories
namespace AuthFlow.Infraestructure.Repositories.Abstract
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Repositories.Interface.Repository;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using AuthFlow.Domain.Entities;
    using AuthFlow.Domain.Interfaces;
    using AuthFlow.Infraestructure.Other;
    using AuthFlow.Persistence.Data;
    using AuthFlow.Persistence.Repositories;
    using System.Linq.Expressions;


    // This class acts as a generic base for all other repository classes. 
    // It contains all the CRUD operation methods that interact with the database.
    public abstract class EntityRepository<T> : Repository<T>, IRepositoryOperations<T> where T : class, IEntity
    {
        protected readonly ILogService _externalLogService;
        // Injecting the database context and the logging service into the constructor
        public EntityRepository(AuthFlowDbContext context, ILogService externalLogService) : base(context)
        {
            _externalLogService = externalLogService;
        }

        // This method adds a new entity to the database after performing a series of validations.
        public new async Task<OperationResult<int>> Add(T entity)
        {
            try
            {
                var hasEntity = await HasEntity(entity);
                if (!hasEntity.IsSuccessful)
                {
                    return hasEntity.ToResultWithIntType();
                }
               
                // Validate the entity
                var validationResult = await AddEntity(entity);
                if (!validationResult.IsSuccessful)
                {
                    return validationResult.ToResultWithIntType();
                }

                // If validation is successful, add the entity to the database
                var addedEntityResult = await base.Add(validationResult.Data);

                // Create a success message and return the success result
                var successMessage = string.Format(Resource.SuccessfullyGeneric, typeof(T).Name);
                return OperationResult<int>.Success(addedEntityResult, successMessage);
            }
            catch(Exception ex)
            {
                var log = Util.GetLogError(ex, entity, OperationExecute.Add);
                var result = await _externalLogService.CreateLog(log);
                if(!result.IsSuccessful)
                {
                    result.ToResultWithIntType();
                }
                
                return OperationResult<int>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }



        // This method modifies an existing entity in the database after performing a series of validations.
        public new async Task<OperationResult<bool>> Modified(T entity)
        {
            try
            {
                var hasEntity = await HasEntity(entity);
                if (!hasEntity.IsSuccessful)
                {
                    return hasEntity.ToResultWithBoolType();
                }

                var resultExist = await ValidateExist(entity.Id);
                if (!resultExist.IsSuccessful)
                {
                    return resultExist.ToResultWithBoolType();
                }

                var resultModifyEntity = await ModifyEntity(entity, resultExist.Data);
                if (!resultModifyEntity.IsSuccessful)
                {
                    return resultModifyEntity.ToResultWithBoolType();
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
                var log = Util.GetLogError(ex, entity, OperationExecute.Modified);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<bool>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        // This method activates an entity by setting its 'Active' status to true.
        public async Task<OperationResult<bool>> Activate(int id)
        {
            try
            {
                
                // Validate if the entity with the provided ID exists
                var validationResult = await ValidateExist(id);

                // If validation is not successful, return a failure operation result
                if (!validationResult.IsSuccessful)
                {
                    return validationResult.ToResultWithBoolType();
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
                var log = Util.GetLogError(ex, id, OperationExecute.Activate);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<bool>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }



        // This method deactivates an entity by setting its 'Active' status to false.
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
                    return OperationResult<bool>.FailureBusinessValidation(messageExist);
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
                var log = Util.GetLogError(ex, id, OperationExecute.Deactivate);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<bool>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        // This method deletes an entity from the database.
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
                    return OperationResult<bool>.FailureBusinessValidation(messageExist);
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
                var log = Util.GetLogError(ex, id, OperationExecute.Remove);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<bool>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        // This method retrieves all entities from the database that match the provided filter expression.
        public new async Task<OperationResult<T>> GetUserById(int id)
        {
            try
            {
                // Get entities from the database based on the provided filter expression
                var validationResult = await ValidateExist(id);

                // If validation is not successful, return a failure operation result
                if (!validationResult.IsSuccessful)
                {
                    return validationResult.ToResultWithGenericType();
                }

                var entity = validationResult.Data;
                // Return a success operation result
                var messageSuccessfully = Resource.SuccessfullyFind;
                return OperationResult<T>.Success(entity, messageSuccessfully);
            }
            catch (Exception ex)
            {
                var log = Util.GetLogError(ex, id, OperationExecute.GetUserById);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<T>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        // This method retrieves all entities from the database that match the provided filter expression.
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
                var log = Util.GetLogError(ex, predicate, OperationExecute.GetAllByFilter);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<IQueryable<T>>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        // This method retrieves a page of entities from the database based on the provided filter expression.
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
                var filterValue = new 
                { 
                    PageNumber = pageNumber, 
                    PageSize = pageSize, 
                    Filter = filter 
                };

                var log = Util.GetLogError(ex, filterValue, OperationExecute.GetPageByFilter);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<IQueryable<T>>.FailureDatabase(Resource.FailedOccurredDataLayer);
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
                var log = Util.GetLogError(ex, filter, OperationExecute.GetCountFilter);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<int>.FailureDatabase(Resource.FailedOccurredDataLayer);
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
                var log = Util.GetLogError(ex, filter, OperationExecute.GetCountFilter);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithIntType();
                }

                return OperationResult<int>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        // This method builds a filter expression based on a provided filter string.
        // Each subclass has to provide its own implementation.
        internal abstract Expression<Func<T, bool>> GetPredicate(string filter);


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
                return OperationResult<T>.FailureBusinessValidation(Resource.FailedNecesaryData);
            }

            return OperationResult<T>.Success(entity,Resource.GlobalOkMessage);
        }


        // Method to validate if an entity exists based on its ID.
        private async Task<OperationResult<T>> ValidateExist(int id)
        {
            // Validate the provided ID
            if (id.Equals(0))
            {
                return OperationResult<T>.FailureBusinessValidation(Resource.FailedNecesaryData);
            }

            // Get the existing user from the repository
            var entityRepo = await base.GetAllByFilter(e => e.Id.Equals(id));
            var entityUnmodified = entityRepo?.FirstOrDefault();
            var hasEntity = entityUnmodified is not null;
            if (!hasEntity)
            {
                var messageExist = string.Format(Resource.GenericExistValidation, typeof(T).Name);
                return OperationResult<T>.FailureBusinessValidation(messageExist);
            }

            // If the entity exists, return a success operation result
            return OperationResult<T>.Success(entityUnmodified, Resource.GlobalOkMessage);
        }
    }
}