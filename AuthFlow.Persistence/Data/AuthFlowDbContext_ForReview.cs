namespace AuthFlow.Persistence.Data
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Persistence.Data.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Represents the main database context for AuthFlow, coordinating Entity Framework functionality.
    /// </summary>
    public class AuthFlowDbContext : DbContext, IAuthFlowDbContext
    {
        /// <summary>
        /// Ensures the database for this context is created. If it exists, no action is taken.
        /// </summary>
        public void Initialize()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Represents a collection of <see cref="User"/> entities in the database context.
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthFlowDbContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the database context.</param>
        public AuthFlowDbContext(DbContextOptions<AuthFlowDbContext> options) : base(options)
        {
            Initialize();
        }

        /// <summary>
        /// Applies the model configurations using the Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder used for configuring the entity model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the User entity properties and relationships.

            // Setting the ID property type and key.
            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnType("int");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

            // Setting the Username property type, requirement, and unique constraint.
            modelBuilder.Entity<User>().Property(u => u.Username).HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.Username, "UC_Users_Username").IsUnique(true);

            // Setting other User properties with their types and requirements.
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.Email, "UC_Users_Email").IsUnique(true);
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.UpdatedAt).HasColumnType("datetime");
            modelBuilder.Entity<User>().Property(u => u.Active).HasColumnType("bit").IsRequired();
        }
    }
}
