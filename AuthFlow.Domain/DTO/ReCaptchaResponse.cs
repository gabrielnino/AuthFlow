// The namespace AuthFlow.Domain.DTO contains all Data Transfer Objects (DTOs) related to 
// the authentication flow in the application. DTOs are objects used to encapsulate data 
// for easier transfer between different system layers.
namespace AuthFlow.Domain.DTO
{
    // The class ReCaptchaResponse represents the response from a reCAPTCHA verification attempt.
    // When a reCAPTCHA token is verified, the service will return a response with information
    // about whether the verification was successful, and any error codes if the verification failed.
    public class ReCaptchaResponse
    {
        // The Success property represents the outcome of the reCAPTCHA verification. If the value
        // is true, it means that the token was successfully verified and the system believes that
        // the user is human. If the value is false, it means that the verification failed.
        public bool Success { get; set; }

        // The ErrorCodes property is a list of error codes that could be returned by the reCAPTCHA 
        // service if the token verification fails. These error codes can provide more detailed 
        // information about the reason for the verification failure. The property is initialized as 
        // an empty list to ensure that it is never null and prevent NullReferenceException.
        public List<string> ErrorCodes { get; set; }
    }
}
