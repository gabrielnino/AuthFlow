using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Validators;
using AuthFlow.Application.Validators.UserValidators;
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

        // Override the ValidateEntity method to provide custom validation logic for the User entity
        protected override async Task<OperationResult<bool>> ValidateEntity(User entity, bool isModified = false)
        {
            // If the entity is null, return a failure operation result with a custom error message
            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.FailedNecesaryData);
            }

            // Create a new instance of the UserValidator and validate the entity
            var validatorAdd = new AddUserRequestRules();
            var validatorModified = new ModifiedUserRequestRules();
            var result = !isModified ? validatorAdd.Validate(entity) : validatorModified.Validate(entity);

            // If the validation fails, return a failure operation result with the validation error message
            if (!result.IsValid)
            {
                var errorMessage = string.Empty;
                foreach (FluentValidation.Results.ValidationFailure error in result.Errors)
                {
                    var messageValidation = string.IsNullOrEmpty(errorMessage) ? error.ErrorMessage : ", " + error.ErrorMessage;
                    errorMessage += messageValidation;
                }
                return OperationResult<bool>.Failure(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            // Check if the email is already registered by another user
            var userByEmail = await base.GetAllByFilter(p => p.Email == entity.Email && p.Id != entity.Id);
            var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            // Check if the username is already registered by another user
            var userByUserName = await base.GetAllByFilter(p => p.Username == entity.Username && p.Id != entity.Id);
            var userExistByUserName = userByUserName?.Data?.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredUser);
            }

            // Return a success operation result
            return OperationResult<bool>.Success(true);
        }

        // Override the CallEntity method to customize the entity before modification
        protected override async Task<OperationResult<User>> ModifyEntity(User entityModified, User entity)
        {
            // Update the username, email, and active status if they are different from the provided entity
            bool hasUsernameChanged = !entity.Username.Equals(entityModified.Username);
            bool hasEmailChanged = !entity.Email.Equals(entityModified.Email);
            bool hasUserOrEmailChanged = hasUsernameChanged || hasEmailChanged;

            if (hasUserOrEmailChanged)
            {
                entity.Username = entityModified.Username;
                entity.Email = entityModified.Email;
                entity.Active = false;
            }

            // Update the UpdatedAt property with the current datetime
            entity.UpdatedAt = DateTime.Now;
            // Custom success message
            var successMessage = string.Format(Resource.SuccessfullySearchGeneric, typeof(User).Name);
            return OperationResult<User>.Success(entityModified, successMessage);
        }
    }
}