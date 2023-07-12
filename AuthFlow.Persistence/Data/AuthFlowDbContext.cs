using AuthFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

// Namespace for Persistence Data
namespace AuthFlow.Persistence.Data
{
    // AuthFlowDbContext is the main class that coordinates Entity Framework functionality for a given data model
    // It manages the entity objects during runtime, which includes populating objects with data from a database,
    // change tracking, and persisting data to the database.
    public class AuthFlowDbContext : DbContext
    {
        public void Initialize()
        {

            Database.EnsureCreated();
            if (!Users.Any())
            {
                foreach (var user in Genesys.GetUsers())
                {
                    Users.Add(user);
                }

                SaveChanges();
            }
        }

        // Define a DbSet for the User model. This represents a collection of Users in the database context.
        public virtual DbSet<User> Users { get; set; }

        // Define a constructor that takes DbContextOptions<AuthFlowDbContext> and passes it to the base constructor.
        // This constructor is used to configure the DbContext with options, which can include a connection string,
        // a provider to use, or other configuration.
        public AuthFlowDbContext(DbContextOptions<AuthFlowDbContext> options) : base(options)
        {
            Initialize();
            // Initialize the Users property with a non-null DbSet<User>
            Users = Set<User>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between User and Session entities

            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnType("int");
            modelBuilder.Entity<User>().Property(u => u.Username).HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.UpdatedAt).HasColumnType("datetime");
            modelBuilder.Entity<User>().Property(u => u.Active).HasColumnType("bit").IsRequired();
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasIndex(u => u.Username, "UC_Users_Username").IsUnique(true);
            modelBuilder.Entity<User>().HasIndex(u => u.Email, "UC_Users_Email").IsUnique(true);
        }

    }
}
