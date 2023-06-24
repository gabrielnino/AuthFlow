using AuthFlow.Domain.Interfaces;

// Namespace holding all the domain entities
namespace AuthFlow.Domain.Entities
{
    /// <summary>
    /// Session is a domain entity that represents a user session in the system.
    /// This entity is used for tracking and managing active user sessions.
    /// Inherits from the IEntity interface.
    /// </summary>
    public class Session : IEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Session. This is typically a primary key in the database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the User that this Session is associated with.
        /// This is typically a foreign key linked to the User table in the database.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the User associated with this session. This property is often used to navigate 
        /// from a Session instance to the associated User instance in object-oriented contexts.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the session token string. This is typically a cryptographically random 
        /// string that uniquely identifies the session.
        /// This property can be null if the session token has not yet been generated.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the Session will expire. After this time, the session will not be valid.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the Session was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether the Session is currently active or not.
        /// If this property is false, the session is considered invalid, regardless of the expiration date.
        /// </summary>
        public bool Active { get; set; }
    }
}
