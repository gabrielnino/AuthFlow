using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Use_cases.Interface.ExternalServices;
using AuthFlow.Application.Validators.UserValidators;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AuthFlow.Infraestructure.Repositories
{
    // UsersRepository is a concrete implementation of IUserRepository
    // It provides repository operations for the User entity using the EntityRepository base class
    public class UsersRepository : EntityRepository<User>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        // The constructor initializes context, externalLogService and configuration properties
        public UsersRepository(AuthFlowDbContext context, ILogService externalLogService, IConfiguration configuration) : base(context, externalLogService)
        {
            _configuration = configuration;
        }

        // Login method is responsible for authenticating the user based on the provided username and password
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

        // AddEntity method adds a new User entity to the database
        // This method also validates the entity before adding it to the database
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

            if(!IsValidEmail(entity?.Email))
            {
                return OperationResult<User>.Failure(Resource.FailedEmailInvalidFormat);
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

        // ModifyEntity method modifies an existing User entity in the database
        // This method also validates the updated entity before updating it in the database
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

            if (!IsValidEmail(entityModified?.Email))
            {
                return OperationResult<User>.Failure(Resource.FailedEmailInvalidFormat);
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

        // GetPredicate method builds a predicate based on the provided filter
        // This predicate can be used to filter the User entities from the database

        internal override Expression<Func<User, bool>> GetPredicate(string filter)
        {
            filter = filter.ToLower();
            return u => string.IsNullOrEmpty(filter) || u.Username.ToLower().Contains(filter) || u.Email.ToLower().Contains(filter);
        }

        // IsValidEmail method checks if the provided email string is a valid email format

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)*\w+[\w-]$";
            return Regex.IsMatch(email, emailPattern);
        }

        // GetUser method creates a new User entity with the provided details

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

        // GetErrorMessage method generates an error message string from a ValidationResult object

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


        // ComputeSha256Hash method generates the SHA256 hash of a provided string
        // This method is used to hash the password before storing it in the database

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

        // GenerateToken method generates a JWT token for the authenticated user

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