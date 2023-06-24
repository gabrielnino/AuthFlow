using AuthFlow.Domain.Interfaces;

// Namespace holding all the domain entities
namespace AuthFlow.Domain.Entities
{
    // Session is a domain entity that represents a user session in the system.
    // This entity is used for tracking and managing active user sessions.
    public class Session : IEntity
    {
        // Unique identifier for the Session
        public int Id { get; set; }

        // Identifier for the User that this Session is associated with
        public int UserId { get; set; }

        public User User { get; set; }

        // The session token string. This may be null if the session token has not been generated.
        public string? Token { get; set; }

        // The date and time when the Session will expire. After this time, the session will not be valid.
        public DateTime Expiration { get; set; }

        // The date and time when the Session was created.
        public DateTime CreatedAt { get; set; }

        // A boolean value indicating whether the Session is currently active or not.
        // If false, the session is considered invalid, regardless of the expiration date.
        public bool Active { get; set; }
    }
}
