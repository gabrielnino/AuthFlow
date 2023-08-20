// The namespace AuthFlow.Application.Repositories.Interface contains the interface definitions 
// for the repository layer in the application. This repository layer handles the interaction between the application 
// and the data source (like a database).
namespace AuthFlow.Application.Repositories.Interface
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Repositories.Interface.Repository;
    using AuthFlow.Domain.Entities;

    /// <summary>
    /// The IUserRepository interface defines the contract for a User repository.
    /// This interface extends IRepositoryOperations<User>, meaning it inherits all the CRUD and other operations 
    /// defined in IRepositoryOperations.
    /// In addition, it includes user-specific operations like Login.
    /// </summary>
    public interface IUsersRepository : IRepositoryOperations<User>
    {
        /// <summary>
        /// The Login method is responsible for checking the user credentials and returning a token if the credentials are correct.
        /// It takes the username and password provided by the user and returns an OperationResult.
        /// The result encapsulates a string which is the token if the login was successful or null if it was unsuccessful.
        /// The implementation of this method should handle the actual process of user authentication.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The result of the operation.</returns>
        Task<OperationResult<string>> Login(string? username, string? password);

        /// <summary>
        /// Method: ValidateEmail
        /// Checks the validity of the provided email.
        /// This method takes in an email, and returns an OperationResult.
        // The specific process of email validation should be implemented in the method that implements this interface.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The result of the operation.</returns>
        Task<OperationResult<bool>> ValidateEmail(string? email);

        /// <summary>
        /// Method: ValidateUsername
        /// Checks the validity of the provided username.
        /// This method takes in a username, and returns an OperationResult.
        /// The specific process of username validation should be implemented in the method that implements this interface.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The result of the operation.</returns>
        Task<OperationResult<Tuple<bool, IEnumerable<string>>>> ValidateUsername(string? username);

        /// <summary>
        /// Method: SetNewPassword
        /// Responsible for setting a new password for the user associated with the provided email.
        /// It takes in an email and a new password, and returns an OperationResult.
        // The implementation should handle the logic of updating the user's password in the data source.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The result of the operation.</returns>
        Task<OperationResult<bool>> SetNewPassword(string? email, string? password);

        /// <summary>
        /// Method: LoginOtp
        /// Handles the process of authenticating a user via a one-time password (OTP).
        /// It takes in the user's email and the OTP provided, then returns an OperationResult.
        /// The result encapsulates a string which is the token if the OTP authentication was successful or null if it was unsuccessful.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="otp">The otp.</param>
        /// <returns>The result of the operation.</returns>
        Task<OperationResult<string>> LoginOtp(string email, string otp);
    }
}
