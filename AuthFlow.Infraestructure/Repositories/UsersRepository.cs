namespace AuthFlow.Infraestructure.Repositories
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Repositories.Interface;
    using AuthFlow.Application.Use_cases.Interface.ExternalServices;
    using AuthFlow.Application.Use_cases.Interface.Operations;
    using AuthFlow.Application.Validators.UserValidators;
    using AuthFlow.Domain.Entities;
    using AuthFlow.Infraestructure.Other;
    using AuthFlow.Infraestructure.Repositories.Abstract;
    using AuthFlow.Persistence.Data;
    using AuthFlow.Persistence.Data.Interface;
    using FluentValidation.Results;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    // UsersRepository is a concrete implementation of IUserRepository
    // It provides repository operations for the User entity using the EntityRepository base class
    public class UsersRepository : EntityRepository<User>, IUsersRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IOTPServices _otpService;
        // The constructor initializes context, externalLogService and configuration properties
        public UsersRepository(AuthFlowDbContext context, ILogService externalLogService, IConfiguration configuration, IOTPServices otpService, IDataSeeder dataSeeder) : base(context, externalLogService, dataSeeder)
        {
            _configuration = configuration;
            _otpService = otpService;
        }
        public async Task<OperationResult<string>> LoginOtp(string email, string otp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
                {
                    return OperationResult<string>.FailureBusinessValidation(Resource.FailedNecesaryData);
                }

                // Get entities from the database based on the provided filter expression
                var result = await base.GetAllByFilter(u => u.Username.Equals(email) || u.Email.Equals(email));
                var user = result?.Data?.FirstOrDefault();
                if (user is null)
                {
                    return OperationResult<string>.FailureBusinessValidation(Resource.FailedUserNotFound);
                }

                var resultOtp = await this._otpService.ValidateOtp(email, otp);
                if(!resultOtp.IsSuccessful)
                {
                    return OperationResult<string>.FailureBusinessValidation(resultOtp.Message);
                }

                var token = GenerateToken(user);

                //// Return a success operation result
                return OperationResult<string>.Success(token, Resource.SuccessfullyLogin);
            }
            catch (Exception ex)
            {
                var loginOTP = new
                {
                    Email = email,
                    Otp = otp
                };

                var log = Util.GetLogError(ex, loginOTP, OperationExecute.LoginOtp);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<string>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        // Login method is responsible for authenticating the user based on the provided username and password
        public async Task<OperationResult<string>> Login(string? username, string? password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return OperationResult<string>.FailureBusinessValidation(Resource.FailedNecesaryData);
                }

                // Get entities from the database based on the provided filter expression
                var result = await base.GetAllByFilter(u => u.Username.Equals(username) || u.Email.Equals(username));
                var user = result?.Data?.FirstOrDefault();
                if (user is null)
                {
                    return OperationResult<string>.FailureBusinessValidation(Resource.FailedUserNotFound);
                }

                var passwordCipher = ComputeSha256Hash(password);
                if (!passwordCipher.Equals(user.Password))
                {
                    return OperationResult<string>.FailureBusinessValidation(Resource.UserFailedPassword);
                }

                var token = GenerateToken(user);

                //// Return a success operation result
                return OperationResult<string>.Success(token, Resource.SuccessfullyLogin);

            }
            catch (Exception ex)
            {
                var login = new
                {
                    Username = username,
                    Password = password
                };
                var log = Util.GetLogError(ex, login, OperationExecute.Login);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<string>.FailureDatabase(Resource.FailedOccurredDataLayer);
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
                return OperationResult<User>.FailureBusinessValidation(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            if (!IsValidEmail(entity?.Email))
            {
                return OperationResult<User>.FailureBusinessValidation(Resource.FailedEmailInvalidFormat);
            }

            // Check if the email is already registered by another user
            var userByEmail = await base.GetAllByFilter(p => p.Email == entity.Email);
            var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<User>.FailureBusinessValidation(Resource.FailedAlreadyRegisteredEmail);
            }

            // Check if the username is already registered by another user
            var userByUserName = await base.GetAllByFilter(p => p.Username.Equals(entity.Username));
            var userExistByUserName = userByUserName?.Data?.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<User>.FailureBusinessValidation(Resource.FailedAlreadyRegisteredUser);
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
                return OperationResult<User>.FailureBusinessValidation(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            if (!IsValidEmail(entityModified?.Email))
            {
                return OperationResult<User>.FailureBusinessValidation(Resource.FailedEmailInvalidFormat);
            }

            // Check if the email is already registered by another user
            var userByEmail = await base.GetAllByFilter(p => p.Email.Equals(entityModified.Email) && !p.Id.Equals(entityModified.Id));
            var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<User>.FailureBusinessValidation(Resource.FailedAlreadyRegisteredEmail);
            }

            // Check if the username is already registered by another user
            var userByUserName = await base.GetAllByFilter(p => p.Username.Equals(entityModified.Username) && !p.Id.Equals(entityModified.Id));
            var userExistByUserName = userByUserName?.Data?.FirstOrDefault();
            if (userExistByUserName is not null)
            {
                return OperationResult<User>.FailureBusinessValidation(Resource.FailedAlreadyRegisteredUser);
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
            return u => string.IsNullOrWhiteSpace(filter) || u.Username.ToLower().Contains(filter) || u.Email.ToLower().Contains(filter);
        }

        // IsValidEmail method checks if the provided email string is a valid email format

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)*\w+[\w-]$";
            return Regex.IsMatch(email, emailPattern);
        }

        // IsUser method checks if the provided username string is a valid username format
        private bool IsUser(string user)
        {
            var emailPattern = @"^[a-zA-Z0-9.]{0,100}$";
            return Regex.IsMatch(user, emailPattern);
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
                var messageValidation = string.IsNullOrWhiteSpace(errorMessage) ? error : ", " + error;
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

        public async Task<OperationResult<bool>> ValidateEmail(string? email)
        {
            try
            {
                if (!IsValidEmail(email))
                {
                    return OperationResult<bool>.FailureBusinessValidation(Resource.FailedEmailInvalidFormat);
                }

                // Check if the email is already registered by another user
                var userByEmail = await base.GetAllByFilter(p => p.Email == email);
                var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
                if (userExistByEmail is not null)
                {
                    return OperationResult<bool>.FailureBusinessValidation(Resource.FailedAlreadyRegisteredEmail);
                }

                return OperationResult<bool>.Success(true, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                var log = Util.GetLogError(ex, email, OperationExecute.ValidateEmail);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<bool>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        private static List<string> GetCommonWords()
        {
            return new List<string>()
            {
                "User",
                "Star",
                "Moon",
                "Sun",
                "Planet",
                "Light",
                "Night",
                "Day",
                "Bright",
                "Dark",
                "Shadow",
                "Dream",
                "Storm",
                "Ocean",
                "Mountain",
                "River",
                "Cloud",
                "Spark",
                "Fire",
                "Ice"
            };
        }

        private static List<string> GenerateUsernameSuggestions(string username)
        {
            var random = new Random();
            var suggestions = new List<string>
            {
                $"{username}.authflow"
            };

            do
            {
                var isNumberSuffix = random.Next(0, 2).Equals(1);
                // Generate a 4-digit random number
                var suffix = isNumberSuffix ? GetNumber(random) : GetCommonWord(random);
                // Append the suffix to the original username
                var suggestion = $"{username}.{suffix}";
                suggestions.Add(suggestion);
            } while (suggestions.Count < 100);

            return suggestions;
        }

        private static string GetNumber(Random random)
        {
            return random.Next(1000, 9999).ToString();
        }

        private static string GetCommonWord(Random random)
        {
            var commonWords = GetCommonWords();
            var indexCommonWord = random.Next(commonWords.Count());
            return commonWords[indexCommonWord].ToLower();
        }

        public async Task<OperationResult<Tuple<bool, IEnumerable<string>>>> ValidateUsername(string? username)
        {
            try
            {
                if (!IsUser(username))
                {
                    return OperationResult<Tuple<bool, IEnumerable<string>>>.FailureBusinessValidation(Resource.FailedUsernameInvalidFormat);
                }

                // Check if the username is already registered by another user
                var userByUserName = await base.GetAllByFilter(p => p.Username.Equals(username));
                var userExistByUserName = userByUserName?.Data?.FirstOrDefault();
                if (userExistByUserName is not null)
                {
                    var userSuggestions = GenerateUsernameSuggestions(username);
                    var userList = new List<string>();
                    var index = 0;
                    do
                    {
                        var userSuggestion = userSuggestions[index];
                        var user = await base.GetAllByFilter(p => p.Username.Equals(userSuggestion));
                        if (user.IsSuccessful.Equals(true) && user.Data.Count().Equals(0))
                        {
                            userList.Add(userSuggestion);
                        }
                        index++;
                    } while (userList.Count()<10);

                    var resultFailed = new Tuple<bool, IEnumerable<string>>(false, userList);
                    return OperationResult<Tuple<bool, IEnumerable<string>>>.Success(resultFailed, Resource.FailedAlreadyRegisteredUser);
                }

                var resultOk = new Tuple<bool, IEnumerable<string>>(true, new List<string>());
                return OperationResult<Tuple<bool, IEnumerable<string>>>.Success(resultOk, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                var log = Util.GetLogError(ex, username, OperationExecute.ValidateUsername);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<Tuple<bool, IEnumerable<string>>>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }

        public async Task<OperationResult<bool>> SetNewPassword(string? email, string? password)
        {
            try
            {
                if (!IsValidEmail(email))
                {
                    return OperationResult<bool>.FailureBusinessValidation(Resource.FailedEmailInvalidFormat);
                }

                // Check if the email is already registered by another user
                var userByEmail = await base.GetAllByFilter(p => p.Email == email);
                var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
                if (userExistByEmail is null)
                {
                    return OperationResult<bool>.FailureBusinessValidation(Resource.FailedNotRegisteredEmail);
                }

                userExistByEmail.Password = password;
                await this.Modified(userExistByEmail);

                return OperationResult<bool>.Success(true, Resource.SuccessfullySetNewPassword);
            }
            catch (Exception ex)
            {
                var employee = new 
                {
                    Email = email,
                    Password = password
                };

                var log = Util.GetLogError(ex, employee, OperationExecute.SetNewPassword);
                var result = await _externalLogService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    result.ToResultWithBoolType();
                }

                return OperationResult<bool>.FailureDatabase(Resource.FailedOccurredDataLayer);
            }
        }
    }
}