using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Domain.Interfaces;
using AuthFlow.Persistence.Data;
using AuthFlow.Persistence.Repositories;
using System.Linq.Expressions;

namespace AuthFlow.Infraestructure.Repositories
{
    public abstract class EntityRepository<T> : Repository<T>, IEntityRepository<T> where T: class, IEntity
    {
        public EntityRepository(AuthFlowDbContext context) : base(context)
        {

        }

        async Task<OperationResult<bool>> IEntityRepository<T>.ActivateEntity(int id)
        {
            try
            {
                var validationResult = await ValidateExist(id, Resource.UserToInactiveNotExist);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }
                var user = validationResult.Data;
                user.Active=true;

                var result = await base.UpdateEntity(user);
                return OperationResult<bool>.Success(result, Resource.SuccessfullyUserActiveated);
            }
            catch //(Exception ex)
            {
                //add log
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        async Task<OperationResult<bool>> IEntityRepository<T>.DeleteEntity(int id)
        {
            try
            {
                var validationResult = await ValidateExist(id, Resource.SuccessfullyUserDeleted);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }
                var user = validationResult.Data;
                bool result = await DeleteEntity(user);
                return OperationResult<bool>.Success(result, Resource.SuccessfullyUserDeleted);
            }
            catch// (Exception ex)
            {
                //add log
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<bool>> DisableEntity(int id)
        {
            try
            {
                var validationResult = await ValidateExist(id, Resource.UserToInactiveNotExist);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult?.Message);
                }
                var user = validationResult.Data;
                user.Active=false;
                var result = await base.UpdateEntity(user);
                return OperationResult<bool>.Success(result, Resource.SuccessfullyUserDisabled);
            }
            catch //(Exception ex)
            {
                //add log
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        async Task<OperationResult<IQueryable<T>>> IEntityRepository<T>.GetEntitiesAll()
        {
            try
            {
                var result = await base.GetAll();
                return OperationResult<IQueryable<T>>.Success(result, Resource.SuccessfullySearch);
            }
            catch// (Exception ex)
            {
                //add log
                return OperationResult<IQueryable<T>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        async Task<OperationResult<int>> IEntityRepository<T>.CreateEntity(T entity)
        {
            try
            {
                var validationResult = await ValidateEntity(entity);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<int>.Failure(validationResult.Message);//modify message
                }

                var result = await base.CreateEntity(entity);
                return OperationResult<int>.Success(result, Resource.SuccessfullyUser);
            }
            catch //(Exception ex)
            {
                //add log
                return OperationResult<int>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        async Task<OperationResult<IQueryable<T>>> IEntityRepository<T>.GetEntitiesByFilter(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var result = await base.GetEntitiesByFilter(predicate);
                return OperationResult<IQueryable<T>>.Success(result, Resource.SuccessfullySearch);
            }
            catch// (Exception ex)
            {
                //add log
                return OperationResult<IQueryable<T>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }


        async Task<OperationResult<bool>> IEntityRepository<T>.UpdateEntity(T entity)
        {
            try
            {
                var validationResult = await ValidateEntity(entity, entity.Id);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult.Message);//modify message
                }

                var result = await base.UpdateEntity(entity);
                return OperationResult<bool>.Success(result, Resource.SuccessfullyUserUpdated);
            }
            catch //(Exception ex)
            {
                //add log
                return OperationResult<bool>.Failure(Resource.FailedOccurredDataLayer);
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