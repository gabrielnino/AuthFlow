//The AuthFlow.Domain.Entities namespace contains all the entity classes used in the authentication flow of the application.
namespace AuthFlow.Domain.Entities
{
    /// <summary>
    /// The TokenResponse class represents a response from a token generating service such as an OAuth or JWT service.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        ///  This is typically a string encoded in a certain format (like JWT) that contains information 
        /// </summary>
        public string Token { get; set; }
    }
}
