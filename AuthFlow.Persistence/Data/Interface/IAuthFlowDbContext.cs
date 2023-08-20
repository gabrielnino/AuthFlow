namespace AuthFlow.Persistence.Data.Interface
{
    using Microsoft.EntityFrameworkCore;
    using User = Domain.Entities.User;

    /// <summary>
    /// Defines a contract for the database context used within the AuthFlow application.
    /// </summary>
    public interface IAuthFlowDbContext
    {
        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges();

        /// <summary>
        /// Gets or sets the collection (table) of users within the context.
        /// </summary>
        DbSet<User> Users { get; set; }
    }
}
