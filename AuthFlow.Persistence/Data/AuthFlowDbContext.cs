using AuthFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

// Namespace for Persistence Data
namespace AuthFlow.Persistence.Data
{
    // AuthFlowDbContext is the main class that coordinates Entity Framework functionality for a given data model
    // It manages the entity objects during run time, which includes populating objects with data from a database,
    // change tracking, and persisting data to the database.
    public class AuthFlowDbContext : DbContext
    {
        // Define a DbSet for the User model. This represents a collection of Users in the database context.
        public DbSet<User> Users { get; set; }

        // Define a DbSet for the Session model. This represents a collection of Sessions in the database context.
        public DbSet<Session> Sessions { get; set; }


        // Define a constructor that takes DbContextOptions<AuthFlowDbContext> and passes it to the base constructor.
        // This constructor is used to configure the DbContext with options, which can include a connection string,
        // a provider to use, or other configuration.
        public AuthFlowDbContext(DbContextOptions<AuthFlowDbContext> options) : base(options)
        {
            // Initialize the Users property with a non-null DbSet<User>
            Users = Set<User>();

            // Initialize the Sessions property with a non-null DbSet<Session>
            Sessions = Set<Session>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Sessions)
                .WithOne(y => y.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
