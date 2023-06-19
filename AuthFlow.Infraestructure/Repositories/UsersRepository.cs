using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Persistence.Data;
using AuthFlow.Persistence.Repositories;
using System.Linq.Expressions;

namespace AuthFlow.Infraestructure.Repositories
{
    public class UsersRepository : Repository<User>, IUserRepository
    {
        public UsersRepository(AuthFlowDbContext context) : base(context)
        {

        }

        public async Task<OperationResult<bool>> UpdateUser(User entity)
        {
            try
            {
                var validationResult = await ValidateUserExistence(entity, entity.Id);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<bool>.Failure(validationResult.Message);
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


        public async Task<OperationResult<int>> CreateUser(User entity)
        {
            try
            {
                var validationResult = await ValidateUserExistence(entity);
                if (!validationResult.IsSuccessful)
                {
                    return OperationResult<int>.Failure(validationResult.Message);
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

        private async Task<OperationResult<bool>> ValidateUserExistence(User entity, int? updatingUserId = null)
        {
            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.NecesaryData);
            }

            var userByEmail = await base.GetEntitiesByFilter(p => p.Email == entity.Email && p.Id != updatingUserId);
            var userExistByEmail = userByEmail.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            var userByUserName = await base.GetEntitiesByFilter(p => p.Username == entity.Username && p.Id != updatingUserId);
            var userExistByUserName = userByUserName.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredUser);
            }

            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<IQueryable<User>>> GetUsersAll()
        {
            try
            {
                var result = await base.GetAll();
                return OperationResult<IQueryable<User>>.Success(result, Resource.SuccessfullySearch);
            }
            catch// (Exception ex)
            {
                //add log
                return OperationResult<IQueryable<User>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<IQueryable<User>>> GetUsersByFilter(Expression<Func<User, bool>> predicate)
        {
            try
            {
                var result = await base.GetEntitiesByFilter(predicate);
                return OperationResult<IQueryable<User>>.Success(result, Resource.SuccessfullySearch);
            }
            catch// (Exception ex)
            {
                //add log
                return OperationResult<IQueryable<User>>.Failure(Resource.FailedOccurredDataLayer);
            }
        }


        private async Task<OperationResult<User>> ValidateUser(int id, string message)
        {
            if (id == 0)
            {
                return OperationResult<User>.Failure(Resource.NecesaryData);
            }

            var userToValidate = await base.GetEntitiesByFilter(p => p.Id == id);
            var user = userToValidate.FirstOrDefault();
            if (user is null)
            {
                return OperationResult<User>.Failure(message);
            }

            return OperationResult<User>.Success(user, string.Empty);
        }

        public async Task<OperationResult<bool>> DeleteUser(int id)
        {
            try
            {
                var validationResult = await ValidateUser(id, Resource.SuccessfullyUserDeleted);
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

        public async Task<OperationResult<bool>> DisableUser(int id)
        {
            try
            {
                var validationResult = await ValidateUser(id, Resource.UserToInactiveNotExist);
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

        public async Task<OperationResult<bool>> ActivateUser(int id)
        {
            try
            {
                var validationResult = await ValidateUser(id, Resource.UserToInactiveNotExist);
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
    }
}