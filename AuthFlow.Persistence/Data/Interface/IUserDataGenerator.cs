namespace AuthFlow.Persistence.Data.Interface
{
    using System.Collections.Immutable;
    using User = Domain.Entities.User;

    /// <summary>
    /// Defines a contract for generating user data for the application.
    /// This can be used for seeding purposes, testing, or any scenario where user data generation is required.
    /// </summary>
    public interface IUserDataGenerator
    {
        /// <summary>
        /// Retrieves an anonymous user object.
        /// </summary>
        /// <returns>An instance of the <see cref="User"/> class.</returns>
        User GetUserAnonymous();

        /// <summary>
        /// Retrieves a predefined list of users.
        /// </summary>
        /// <returns>An immutable list of <see cref="User"/> objects.</returns>
        ImmutableList<User> GetUsers();

        /// <summary>
        /// Generates a massive list of users with randomized data based on the given size parameters.
        /// </summary>
        /// <param name="sizeFirstNames">The number of different first names to use in generation.</param>
        /// <param name="sizeMiddleName">The number of different middle names to use in generation.</param>
        /// <param name="sizeLastname">The number of different last names to use in generation.</param>
        /// <param name="sizeSecondlastName">The number of different second last names to use in generation.</param>
        /// <returns>An immutable list of <see cref="User"/> objects.</returns>
        ImmutableList<User> GenerateMassiveUserList(
            int sizeFirstNames = 300,
            int sizeMiddleName = 10,
            int sizeLastname = 10,
            int sizeSecondlastName = 10);
    }
}
