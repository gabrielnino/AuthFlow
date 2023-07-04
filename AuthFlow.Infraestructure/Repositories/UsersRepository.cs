using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Uses_cases.Interface;
using AuthFlow.Application.Validators.UserValidators;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;
using FluentValidation.Results;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AuthFlow.Infraestructure.Repositories
{
    // UsersRepository is a concrete implementation of IUserRepository
    // It provides repository operations for the User entity using the EntityRepository base class
    public class UsersRepository : EntityRepository<User>, IUserRepository
    {
        
        public UsersRepository(AuthFlowDbContext context, IExternalLogService externalLogService) : base(context, externalLogService)
        {
           
        }

        // Override the ValidateEntity method to provide custom validation logic for the User entity
        internal override async Task<OperationResult<User>> AddEntity(User entity)
        {

            // Create a new instance of the UserValidator and validate the entity
            var validatorAdd = new AddUserRequestRules();
            var result = validatorAdd.Validate(entity);

            // If the validation fails, return a failure operation result with the validation error message
            if (!result.IsValid)
            {
                var errorMessage = GetErrorMessage(result);
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

            User entityAdd = GetUser(entity);

            // Return a success operation result
            return OperationResult<User>.Success(entityAdd);
        }

        private static User GetUser(User entity)
        {
            return new User()
            {
                Username = entity.Username,
                Password = ComputeSha256Hash(entity.Password),
                Email = entity.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Active = false,
            };
        }

        // Override the CallEntity method to customize the entity before modification T entityModified, T entityUnmodified
        internal override async Task<OperationResult<User>> ModifyEntity(User entityModified, User entityUnmodified)
        {
            var validatorModified = new ModifiedUserRequestRules();
            var result = validatorModified.Validate(entityModified);
            // If the validation fails, return a failure operation result with the validation error message
            if (!result.IsValid)
            {
                var errorMessage = GetErrorMessage(result);
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
            entityUnmodified.Password =  ComputeSha256Hash(entityModified.Password);
            // Custom success message
            var successMessage = string.Format(Resource.SuccessfullySearchGeneric, typeof(User).Name);
            return OperationResult<User>.Success(entityUnmodified, successMessage);
        }

        private static string GetErrorMessage(ValidationResult result)
        {
            var errors = result.Errors.Select(x => x.ErrorMessage).Distinct();
            var errorMessage = string.Empty;
            foreach (var error in errors)
            {
                var messageValidation = string.IsNullOrEmpty(errorMessage) ? error : ", " + error;
                errorMessage += messageValidation;
            }

            return errorMessage;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using SHA256 sha256Hash = SHA256.Create();
            // ComputeHash - returns byte array  
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            var builder = new StringBuilder();
            foreach (byte v in bytes)
            {
                builder.Append(v.ToString("x2"));
            }

            return builder.ToString();
        }

        internal override Expression<Func<User, bool>> GetPredicate(string filter)
        {
            return u =>  Compare(u.Username, u.Email, filter);
        }

        private static bool Compare(string username, string email, string filter)
        {
            username = GetValidate(username);
            email = GetValidate(email);
            filter = GetValidate(filter);
            return string.IsNullOrEmpty(filter) || username.Contains(filter) || email.Contains(filter);
        }

        private static string GetValidate(string value)
        {
            return value is null ? string.Empty :
                value.ToLower()
                .Trim()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");
        }
    }
}