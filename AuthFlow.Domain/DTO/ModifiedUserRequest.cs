namespace AuthFlow.Domain.DTO
{
    public class ModifiedUserRequest
    {
        // The username of the User. This may be null if the username has not been set.
        public string? Username { get; set; }

        // The password of the User. This should be stored securely, ideally hashed. 
        // It may be null if the password has not been set.
        public string? Password { get; set; }

        // The email of the User. This may be null if the email has not been set.
        public string? Email { get; set; }

    }
}
