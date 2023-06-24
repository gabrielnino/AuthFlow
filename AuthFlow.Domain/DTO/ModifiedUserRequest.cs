namespace AuthFlow.Domain.DTO
{
    /// <summary>
    /// Represents a request to modify an existing user's information in the system. 
    /// This can be used for operations like user profile updates.
    /// </summary>
    public class ModifiedUserRequest
    {
        /// <summary>
        /// Gets or sets the username of the user. This property serves as a unique identifier
        /// for a user in a system and is used for user logins. 
        /// This property can be null if the username is not intended to be modified in this request.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user. The password should be stored securely, 
        /// ideally using a strong hashing algorithm. 
        /// This property can be null if the password is not intended to be modified in this request.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the email of the user. This can be used for communication with the user,
        /// as well as a recovery method for account access. 
        /// This property can be null if the email is not intended to be modified in this request.
        /// </summary>
        public string? Email { get; set; }
    }
}
