namespace AuthFlow.Domain.DTO
{
    /// <summary>
    /// Represents a request to add a new user in the system. This is used in the user registration process.
    /// </summary>
    public class AddUserRequest
    {
        /// <summary>
        /// Gets or sets the username of the user. This property serves as a unique identifier
        /// for a user in a system and is used for user logins. 
        /// This property can be null if the username has not yet been set.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user. The password should be stored securely, 
        /// ideally using a strong hashing algorithm. 
        /// This property can be null if the password has not yet been set.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the email of the user. This can be used for communication with the user,
        /// as well as a recovery method for account access. 
        /// This property can be null if the email has not yet been set.
        /// </summary>
        public string? Email { get; set; }
    }
}
