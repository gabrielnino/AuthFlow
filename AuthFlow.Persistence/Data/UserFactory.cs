namespace AuthFlow.Persistence.Data
{
    using AuthFlow.Persistence.Data.Interface;
    using User = Domain.Entities.User;
    public class UserFactory : IUserFactory
    {
        /// <summary>
        /// Creates a user entity.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="passwordHash">The passwordHash.</param>
        /// <param name="email">The email.</param>
        /// <param name="creationDateTime">The creation datetime.</param>
        /// <returns>The instance of user.</returns>
        public User CreateUser(string username, string passwordHash, string email, DateTime creationDateTime)
        {
            return new User
            {
                Username = username,
                Password = passwordHash,
                Email = email,
                CreatedAt = creationDateTime,
                UpdatedAt = creationDateTime,
                Active = true
            };
        }
    }

}
