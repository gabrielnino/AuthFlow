using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Validators;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;

namespace AuthFlow.Infraestructure.Repositories
{
    public class UsersRepository : EntityRepository<User>, IUserRepository
    {
        public UsersRepository(AuthFlowDbContext context) : base(context)
        {
        }

        protected override async Task<OperationResult<bool>> ValidateEntity(User entity, int? updatingUserId = null)
        {
            var validator = new UserValidator();
            var result = validator.Validate(entity);
            if(!result.IsValid)
            {
                var errorMessage = string.Empty;
                foreach (FluentValidation.Results.ValidationFailure? error in result.Errors)
                {
                    errorMessage += ", " + error.ErrorMessage;
                }
                return OperationResult<bool>.Failure(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.NecesaryData);
            }

            var userByEmail = await base.GetAllByFilter(p => p.Email == entity.Email && p.Id != updatingUserId);
            var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            var userByUserName = await base.GetAllByFilter(p => p.Username == entity.Username && p.Id != updatingUserId);
            var userExistByUserName = userByUserName?.Data?.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredUser);
            }

            return OperationResult<bool>.Success(true);
        }
    }
}