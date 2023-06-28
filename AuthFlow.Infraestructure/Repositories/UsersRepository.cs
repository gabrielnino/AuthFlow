using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Validators;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;

namespace AuthFlow.Infraestructure.Repositories
{
    // UsersRepository is a concrete implementation of IUserRepository
    // It provides repository operations for the User entity using the EntityRepository base class
    public class UsersRepository : EntityRepository<User>, IUserRepository
    {
        public UsersRepository(AuthFlowDbContext context) : base(context)
        {
        }

        // Override the CallEntity method to customize the entity before modification
        protected override async Task<OperationResult<User>> CallEntity(User entity)
        {
            // Get the existing user from the repository
            var userRepo = await base.GetAllByFilter(e => e.Id.Equals(entity.Id));
            var user = userRepo?.Data?.FirstOrDefault();
            bool hasUser = user is not null;
            if (!hasUser)
            {
                return OperationResult<User>.Failure(Resource.FailedUserDoesNotExist);
            }

            // Update the username, email, and active status if they are different from the provided entity
            bool hasUsernameChanged = !user.Username.Equals(entity.Username);
            bool hasEmailChanged = !user.Email.Equals(entity.Email);
            bool hasUserOrEmailChanged = hasUsernameChanged || hasEmailChanged;

            if (hasUserOrEmailChanged)
            {
                user.Username = entity.Username;
                user.Email = entity.Email;
                user.Active = false;
            }

            // Update the UpdatedAt property with the current datetime
            user.UpdatedAt = DateTime.Now;
            // Custom success message
            var messageSuccessfully = string.Format(Resource.SuccessfullySearchGeneric, typeof(User).Name);
            return OperationResult<User>.Success(entity, messageSuccessfully);
        }

        // Override the ValidateEntity method to provide custom validation logic for the User entity
        protected override async Task<OperationResult<bool>> ValidateEntity(User entity, int? updatingUserId = null)
        {
            // Check if the entity is being modified or added
            var isModified = updatingUserId != null;

            // Create a new instance of the UserValidator and validate the entity
            var validator = new UserValidator(isModified);
            var result = validator.Validate(entity);

            // If the validation fails, return a failure operation result with the validation error message
            if (!result.IsValid)
            {
                var errorMessage = string.Empty;
                foreach (FluentValidation.Results.ValidationFailure error in result.Errors)
                {
                    errorMessage += ", " + error.ErrorMessage;
                }
                return OperationResult<bool>.Failure(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            // If the entity is null, return a failure operation result with a custom error message
            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.FailedNecesaryData);
            }

            // Check if the email is already registered by another user
            var userByEmail = await base.GetAllByFilter(p => p.Email == entity.Email && p.Id != updatingUserId);
            var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            // Check if the username is already registered by another user
            var userByUserName = await base.GetAllByFilter(p => p.Username == entity.Username && p.Id != updatingUserId);
            var userExistByUserName = userByUserName?.Data?.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredUser);
            }

            // Return a success operation result
            return OperationResult<bool>.Success(true);
        }
    }
}