// Define a namespace for authentication flow-related DTOs (Data Transfer Objects).
namespace AuthFlow.Application.DTOs
{
    // Represents an exception for invalid operation results.
    public class InvalidOperationResultException : Exception
    {
        // Constructor for the exception that accepts an error message.
        public InvalidOperationResultException(string message)
            : base(message)  // Call the base exception class with the provided message.
        {
        }
    }
}
