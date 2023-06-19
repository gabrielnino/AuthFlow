using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Domain.Interfaces;
using AuthFlow.Persistence.Data;
using AuthFlow.Persistence.Repositories;
using System.Linq.Expressions;

namespace AuthFlow.Infraestructure.Repositories
{
    public abstract class EntityRepository<T> : Repository<T>, IRepositoryOperations<T> where T : class, IEntity
    {
        public EntityRepository(AuthFlowDbContext context) : base(context)
        {

        }

        public async Task<OperationResult<bool>> Activate(int id)
        {
            try
            {
                var messageExist = string.Format(Resource.GenericToInactiveNotExist, typeof(T).Name);
                var validationResult = await ValidateExist(id, messageExist);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }
                var user = validationResult.Data;
                user.Active=true;

                var result = await base.UpdateEntity(user);
                var messageSuccess = string.Format(Resource.SuccessfullyGenericActiveated, typeof(T).Name);
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch //(Exception ex)
            {
                //add log
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<int>> Add(T entity)
        {
            try
            {
                var validationResult = await ValidateEntity(entity);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<int>.Failure(validationResult.Message);//modify message
                }

                var result = await base.CreateEntity(entity);
                var messageExist = string.Format(Resource.SuccessfullyGeneric, typeof(T).Name);
                return OperationResult<int>.Success(result, messageExist);
            }
            catch //(Exception ex)
            {
                //add log
                return OperationResult<int>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<bool>> Deactivate(int id)
        {
            try
            {
                var messageExist = string.Format(Resource.UserToInactiveNotExist, typeof(T).Name);
                var validationResult = await ValidateExist(id, messageExist);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }
                var user = validationResult.Data;
                user.Active=false;
                var result = await base.UpdateEntity(user);
                var messageSuccess = string.Format(Resource.SuccessfullyGenericDisabled, typeof(T).Name);
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch //(Exception ex)
            {
                //add log
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<bool>> Modify(T entity)
        {
            try
            {
                var validationResult = await ValidateEntity(entity, entity.Id);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult.Message);//modify message
                }

                var result = await base.UpdateEntity(entity);
                var messageSuccess = string.Format(Resource.SuccessfullyGenericUpdated, typeof(T).Name);
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch //(Exception ex)
            {
                //add log
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<bool>> Remove(int id)
        {
            try
            {
                var messageDeleted= string.Format(Resource.SuccessfullyGenericDeleted, typeof(T).Name);
                var validationResult = await ValidateExist(id, messageDeleted);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }
                var user = validationResult.Data;
                bool result = await DeleteEntity(user);
                var messageSuccess = string.Format(Resource.SuccessfullyGenericDeleted, typeof(T).Name);
                return OperationResult<bool>.Success(result, messageSuccess);
            }
            catch// (Exception ex)
            {
                //add log
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<IQueryable<T>>> RetrieveAll()
        {
            try
            {
                var result = await base.GetAll();
                var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);
                return OperationResult<IQueryable<T>>.Success(result, messageSuccessfully);
            }
            catch// (Exception ex)
            {
                //add log
                return OperationResult<IQueryable<T>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<IQueryable<T>>> RetrieveByFilter(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var result = await base.GetEntitiesByFilter(predicate);
                var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(T).Name);
                return OperationResult<IQueryable<T>>.Success(result, messageSuccessfully);
            }
            catch// (Exception ex)
            {
                //add log
                return OperationResult<IQueryable<T>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        protected abstract Task<OperationResult<bool>> ValidateEntity(T entity, int? updatingUserId = null);

        private async Task<OperationResult<T>> ValidateExist(int id, string message)
        {
            if (id == 0)
            {
                return OperationResult<T>.Failure(Resource.NecesaryData);
            }

            var userToValidate = await base.GetEntitiesByFilter(p => p.Id == id);
            var user = userToValidate.FirstOrDefault();
            if (user is null)
            {
                return OperationResult<T>.Failure(message);
            }

            return OperationResult<T>.Success(user, string.Empty);
        }
    }
}