namespace AuthFlow.Domain.DTO
{
    /// <summary>
    /// Represents a request to add a new session for a user. This is used to handle user authentication process.
    /// </summary>
    public class AddSessionRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the User that this Session is associated with.
        /// This is typically a primary key from the user data in your database.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the session token string. This is typically a cryptographically random 
        /// string that uniquely identifies the session.
        /// This property can be null if the session token has not yet been generated.
        /// </summary>
        public string? Token { get; set; }
    }
}
