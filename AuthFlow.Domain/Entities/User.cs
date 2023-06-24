using AuthFlow.Domain.Interfaces;

// Namespace holding all the domain entities
namespace AuthFlow.Domain.Entities
{
    // User is a domain entity that represents a user in the system.
    // This entity contains information necessary for user authentication and identification.
    public class User : IEntity
    {
        // Unique identifier for the User
        public int Id { get; set; }

        // The username of the User. This may be null if the username has not been set.
        public string? Username { get; set; }

        // The password of the User. This should be stored securely, ideally hashed. 
        // It may be null if the password has not been set.
        public string? Password { get; set; }

        // The email of the User. This may be null if the email has not been set.
        public string? Email { get; set; }

        // The date and time when the User was created.
        public DateTime CreatedAt { get; set; }

        // The date and time when the User was last updated.
        // This may be null if the user has not been updated since creation.
        public DateTime? UpdatedAt { get; set; }

        // A boolean value indicating whether the User account is currently active or not.
        // If false, the user account is considered disabled or deactivated.
        public bool Active { get; set; }

        public List<Session> Sessions { get; set; }
    }
}
