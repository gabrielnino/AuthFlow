using AuthFlow.Domain.Interfaces;

// Namespace holding all the domain entities
namespace AuthFlow.Domain.Entities
{
    // AccessToken is a domain entity that represents the access token information in the system.
    // This entity is used for authorization/authentication purposes.
    public class AccessToken : IEntity
    {
        // Unique identifier for the AccessToken
        public int Id { get; set; }

        // Identifier for the User that this AccessToken is associated with
        public int UserId { get; set; }

        // The actual access token string. This may be null if the token has not been generated.
        public string? Token { get; set; }

        // The date and time when the AccessToken will expire. After this time, the token will not be valid.
        public DateTime Expiration { get; set; }

        // The date and time when the AccessToken was created.
        public DateTime CreatedAt { get; set; }

        // A boolean value indicating whether the AccessToken is currently active or not.
        // If false, the token is considered invalid, regardless of the expiration date.
        public bool Active { get; set; }
    }
}
