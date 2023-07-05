using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface.Repository;
using AuthFlow.Domain.Entities;

// Namespace for application repository interfaces
namespace AuthFlow.Application.Repositories.Interface
{
    // Interface for the User repository.
    // This interface defines methods for interacting with the User data in the repository.
    public interface IUserRepository : IRepositoryOperations<User>
    {
        Task<OperationResult<string>> Login(string? username, string? password);
    }
}
