namespace AuthFlow.Application.DTOs
{
    /// <summary>
    /// Enumerates different types of application errors.
    /// </summary>
    public enum ErrorTypes_REVIEWED
    {
        None,  // Represents no error.
        BusinessValidationError,  // Represents errors related to business logic validation.
        DatabaseError,  // Represents errors when interacting with the database.
        ExternalServicesError,  // Represents errors when interacting with external services.
        UnexpectedError,  // Represents any unexpected or unclassified errors.
        DataSubmittedInvalid,  // Represents errors due to invalid data submission.
        ConfigurationMissingError,  // Represents errors due to missing configurations.

        NetworkError,  // Represents errors due to network issues.
        UserInputError,  // Represents errors related to user input.
        NotFoundError,  // Represents errors where a requested resource is not found.
        AuthenticationError,  // Represents errors related to user authentication.
        AuthorizationError,  // Represents errors related to user authorization or permissions.
        ResourceError,  // Represents errors related to resource allocation or access.
        TimeoutError  // Represents errors due to operation timeouts.
    }
}
