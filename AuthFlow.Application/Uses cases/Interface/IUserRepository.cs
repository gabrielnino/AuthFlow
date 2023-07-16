using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface.Repository;
using AuthFlow.Domain.Entities;

// The namespace AuthFlow.Application.Repositories.Interface contains the interface definitions 
// for the repository layer in the application. This repository layer handles the interaction between the application 
// and the data source (like a database).
namespace AuthFlow.Application.Repositories.Interface
{
    // The IUserRepository interface defines the contract for a User repository.
    // This interface extends IRepositoryOperations<User>, meaning it inherits all the CRUD and other operations 
    // defined in IRepositoryOperations.
    // In addition, it includes user-specific operations like Login.
    public interface IUserRepository : IRepositoryOperations<User>
    {
        // The Login method is responsible for checking the user credentials and returning a token if the credentials are correct.
        // It takes the username and password provided by the user and returns an OperationResult.
        // The result encapsulates a string which is the token if the login was successful or null if it was unsuccessful.
        // The implementation of this method should handle the actual process of user authentication.
        Task<OperationResult<string>> Login(string? username, string? password);
    }
}
