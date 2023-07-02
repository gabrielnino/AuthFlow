using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Validators;
using AuthFlow.Application.Validators.UserValidators;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

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
        protected override async Task<OperationResult<User>> AddEntity(User entity)
        {

            // Create a new instance of the UserValidator and validate the entity
            var validatorAdd = new AddUserRequestRules();
            var result = validatorAdd.Validate(entity);

            // If the validation fails, return a failure operation result with the validation error message
            if (!result.IsValid)
            {
                var errorMessage = string.Empty;
                foreach (var error in result.Errors.Select(x => x.ErrorMessage).Distinct())
                {
                    var messageValidation = string.IsNullOrEmpty(errorMessage) ? error : ", " + error;
                    errorMessage += messageValidation;
                }
                return OperationResult<User>.Failure(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            // Check if the email is already registered by another user
            var userByEmail = await base.GetAllByFilter(p => p.Email == entity.Email);
            var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<User>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            // Check if the username is already registered by another user
            var userByUserName = await base.GetAllByFilter(p => p.Username.Equals(entity.Username));
            var userExistByUserName = userByUserName?.Data?.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<User>.Failure(Resource.FailedAlreadyRegisteredUser);
            }

            var entityAdd = new User()
            {
                Username = entity.Username,
                Password = entity.Password,
                Email = entity.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Active = false,
            };

            // Return a success operation result
            return OperationResult<User>.Success(entityAdd);
        }

        // Override the CallEntity method to customize the entity before modification T entityModified, T entityUnmodified
        protected override async Task<OperationResult<User>> ModifyEntity(User entityModified, User entityUnmodified)
        {
            var validatorModified = new ModifiedUserRequestRules();
            var result = validatorModified.Validate(entityModified);
            // If the validation fails, return a failure operation result with the validation error message
            if (!result.IsValid)
            {
                var errorMessage = string.Empty;
                foreach (var error in result.Errors.Select(x => x.ErrorMessage).Distinct())
                {
                    var messageValidation = string.IsNullOrEmpty(errorMessage) ? error : ", " + error;
                    errorMessage += messageValidation;
                }
                return OperationResult<User>.Failure(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            // Check if the email is already registered by another user
            var userByEmail = await base.GetAllByFilter(p => p.Email.Equals(entityModified.Email) && !p.Id.Equals(entityModified.Id));
            var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<User>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            // Check if the username is already registered by another user
            var userByUserName = await base.GetAllByFilter(p => p.Username.Equals(entityModified.Username) && !p.Id.Equals(entityModified.Id));
            var userExistByUserName = userByUserName?.Data?.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<User>.Failure(Resource.FailedAlreadyRegisteredUser);
            }

            // Update the username, email, and active status if they are different from the provided entity
            bool hasUsernameChanged = !entityModified.Username.Equals(entityUnmodified.Username);
            bool hasEmailChanged = !entityModified.Email.Equals(entityUnmodified.Email);
            bool hasUserOrEmailChanged = hasUsernameChanged || hasEmailChanged;

            if (hasUserOrEmailChanged)
            {
                entityUnmodified.Username = entityModified.Username;
                entityUnmodified.Email = entityModified.Email;
                entityUnmodified.Active = false;
            }

            // Update the UpdatedAt property with the current datetime
            entityUnmodified.UpdatedAt = DateTime.Now;
            entityUnmodified.Password = entityModified.Password;
            // Custom success message
            var successMessage = string.Format(Resource.SuccessfullySearchGeneric, typeof(User).Name);
            return OperationResult<User>.Success(entityModified, successMessage);
        }
    }
}