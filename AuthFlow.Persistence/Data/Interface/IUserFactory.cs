namespace AuthFlow.Persistence.Data.Interface
{
    using System;
    using User = Domain.Entities.User;

    /// <summary>
    /// Defines a contract for creating user instances for the application.
    /// This interface abstracts the creation logic of <see cref="User"/> entities.
    /// </summary>
    public interface IUserFactory
    {
        /// <summary>
        /// Creates a new user instance with the given parameters.
        /// </summary>
        /// <param name="username">The username for the new user.</param>
        /// <param name="passwordHash">The hashed password for the new user.</param>
        /// <param name="email">The email address for the new user.</param>
        /// <param name="creationDateTime">The creation date and time for the new user.</param>
        /// <returns>An instance of the <see cref="User"/> class with the provided details.</returns>
        User CreateUser(string username, string passwordHash, string email, DateTime creationDateTime);
    }
}
