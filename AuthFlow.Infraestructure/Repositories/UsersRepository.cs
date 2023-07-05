using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Uses_cases.Interface;
using AuthFlow.Application.Validators.UserValidators;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AuthFlow.Infraestructure.Repositories
{
    // UsersRepository is a concrete implementation of IUserRepository
    // It provides repository operations for the User entity using the EntityRepository base class
    public class UsersRepository : EntityRepository<User>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        public UsersRepository(AuthFlowDbContext context, IExternalLogService externalLogService, IConfiguration configuration) : base(context, externalLogService)
        {
            _configuration = configuration;
        }

        public async Task<OperationResult<string>> Login(string? username, string? password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return OperationResult<string>.Failure(Resource.FailedNecesaryData);
                }

                // Get entities from the database based on the provided filter expression
                var result = await base.GetAllByFilter(u => u.Username.Equals(username));
                var user = result?.Data?.FirstOrDefault();
                if (user is null)
                {
                    return OperationResult<string>.Failure(Resource.FailedUserNotFound);
                }

                var passwordCipher = ComputeSha256Hash(password);
                if (!passwordCipher.Equals(user.Password))
                {
                    return OperationResult<string>.Failure(Resource.UserFailedPassword);
                }
                
                var token = GenerateToken(user);

                //// Return a success operation result
                return OperationResult<string>.Success(token, Resource.SuccessfullyLogin);

            }
            catch (Exception ex)
            {
                var log = GetLogError(ex, "GetByFilter", OperationExecute.GetAllByFilter);
                await _externalLogService.CreateLog(log);
                return OperationResult<string>.Failure(Resource.FailedOccurredDataLayer);
            }
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

        internal override Expression<Func<User, bool>> GetPredicate(string filter)
        {
            filter = filter.ToLower();
            return u => string.IsNullOrEmpty(filter) || u.Username.ToLower().Contains(filter) || u.Email.ToLower().Contains(filter);
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

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                // new Claim("AdminType", admin.AdminType),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securitytoken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(securitytoken);
        }
    }
}