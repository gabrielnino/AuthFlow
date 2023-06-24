using AuthFlow.Application.Repositories.Interface.Repository;
using AuthFlow.Domain.Entities;

// Namespace for application repository interfaces
namespace AuthFlow.Application.Repositories.Interface
{
    // Interface for the Session repository.
    // This interface defines methods for interacting with the Session data in the repository.
    public interface ISessionRepository : IRepositoryOperations<Session>
    {
        public IUserRepository _userRepository { get; set; }
    }
}
