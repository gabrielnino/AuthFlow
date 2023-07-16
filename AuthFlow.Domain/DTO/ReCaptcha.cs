// The namespace AuthFlow.Domain.DTO contains all Data Transfer Objects (DTOs) that are
// related to the authentication flow in the application. DTOs are simple objects that
// are used to encapsulate data and send it from one layer of the system to another.
namespace AuthFlow.Domain.DTO
{
    // The class ReCaptcha represents a reCAPTCHA token. This token is used to verify that the
    // user interacting with the system is a human and not a bot. reCAPTCHA is a Google service
    // that helps protect websites from spam and abuse.
    public class ReCaptcha
    {
        // The Token property represents a reCAPTCHA token. This token is produced when a user
        // successfully completes a reCAPTCHA challenge on the client side. The token is then sent
        // to the server where it can be validated to ensure that the interaction was legitimate.
        // The "?" after "string" means that this property is nullable. It can be null if the user 
        // hasn't completed a reCAPTCHA challenge yet.
        public string? Token { get; set; }
    }
}
