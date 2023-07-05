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
                var commonPassword = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";
                var users = new List<User>
                {
                    new User { Username = "luis.nino", Password = commonPassword, Email = "luis.nino@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "maria.perez", Password = commonPassword, Email = "maria.perez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "juan.gomez", Password = commonPassword, Email = "juan.gomez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "ana.sanchez", Password = commonPassword, Email = "ana.sanchez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "pedro.rodriguez", Password = commonPassword, Email = "pedro.rodriguez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "carla.mendez", Password = commonPassword, Email = "carla.mendez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "pablo.martinez", Password = commonPassword, Email = "pablo.martinez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "sara.torres", Password = commonPassword, Email = "sara.torres@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "lucas.fernandez", Password = commonPassword, Email = "lucas.fernandez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "marina.castro", Password = commonPassword, Email = "marina.castro@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "diego.cortez", Password = commonPassword, Email = "diego.cortez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "eva.gonzalez", Password = commonPassword, Email = "eva.gonzalez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "andres.morales", Password = commonPassword, Email = "andres.morales@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    new User { Username = "irene.gil", Password = commonPassword, Email = "irene.gil@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                    // Añade más usuarios aquí...
                };

                foreach (var user in users)
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
            // Initialize the Users property with a non-null DbSet<User>
            Users = Set<User>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between User and Session entities
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }

    }
}
