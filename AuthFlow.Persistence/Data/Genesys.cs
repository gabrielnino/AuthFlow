using AuthFlow.Domain.Entities;

namespace AuthFlow.Persistence.Data
{
    // The Genesys class is an internal helper class used for generating initial user data.
    internal class Genesys
    {
        // GetUsers method generates a list of User entities to be initially populated in the database.
        public static List<User> GetUsers()
        {
            // Common password for all initial users.
            var commonPassword = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";

            // Define initial users
            var users = new List<User>
            {
                // The properties of each user are set here including the Username, Password, Email, CreatedAt, UpdatedAt, and Active status.
                new User { Username = "usernameanonymous", Password = "72b28030ce99fa4d0ab678f1d4a4374cc0d7bb676eb4307b0fa105f4c66b644e", Email = "user.anonymous@withoutemail.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = true },
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
                };
            // Return the list of initial users
            return users;
        }
    }
}
