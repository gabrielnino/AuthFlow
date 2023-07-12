namespace AuthFlow.Domain.DTO
{
    public class Credential
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
    }
}
