namespace AuthFlow.Application.DTOs
{
    /// <summary>
    /// Provides extension methods for the ErrorTypes enum.
    /// </summary>
    public static class HelperErrorTypesExtensions
    {
        /// <summary>
        /// Gets the string representation of the specified error type.
        /// </summary>
        /// <param name="error">The error type.</param>
        /// <returns>The string representation of the error type.</returns>
        public static string ToErrorString(this ErrorTypes error)
        {
            return error switch
            {
                ErrorTypes.None => "NONE",
                ErrorTypes.BusinessValidationError => "BUSINESS_VALIDATION_ERROR",
                ErrorTypes.DatabaseError => "DATABASE_ERROR",
                ErrorTypes.ExternalServicesError => "EXTERNAL_SERVICE_ERROR",
                ErrorTypes.UnexpectedError => "UNEXPECTED_ERROR",
                ErrorTypes.DataSubmittedInvalid => "DATA_SUBMITTED_INVALID",
                ErrorTypes.ConfigurationMissingError => "CONFIGURATION_MISSING_ERROR",
                
                ErrorTypes.NetworkError => "NETWORK_ERROR",
                ErrorTypes.UserInputError => "USER_INPUT_ERROR",
                ErrorTypes.NotFoundError => "NOT_FOUND_ERROR",
                ErrorTypes.AuthenticationError => "AUTHENTICATION_ERROR",
                ErrorTypes.AuthorizationError => "AUTHORIZATION_ERROR",
                ErrorTypes.ResourceError => "RESOURCE_ERROR",
                ErrorTypes.TimeoutError => "TIMEOUT_ERROR",
                _ => "UNKNOWN_ERROR",
            };
        }

        /// <summary>
        /// Gets a user-friendly description for the specified error type.
        /// </summary>
        /// <param name="error">The error type.</param>
        /// <returns>The description of the error type.</returns>
        public static string GetDescription(this ErrorTypes error)
        {
            return error switch
            {
                ErrorTypes.None => "No error occurred.",
                ErrorTypes.BusinessValidationError => "A business validation error occurred.",
                ErrorTypes.DatabaseError => "An error occurred while accessing the database.",
                ErrorTypes.ExternalServicesError => "An error occurred with an external service.",
                ErrorTypes.UnexpectedError => "An unexpected error occurred.",
                ErrorTypes.DataSubmittedInvalid => "The data submitted was invalid or in an incorrect format.",
                ErrorTypes.ConfigurationMissingError => "Required configuration data is missing.",

                ErrorTypes.NetworkError => "An error occurred due to network issues.",
                ErrorTypes.UserInputError => "There was an error with the user's input.",
                ErrorTypes.NotFoundError => "The requested resource was not found.",
                ErrorTypes.AuthenticationError => "An authentication error occurred. User credentials might be incorrect or expired.",
                ErrorTypes.AuthorizationError => "The user does not have the necessary permissions to perform the requested operation.",
                ErrorTypes.ResourceError => "There was an error related to resource allocation or access.",
                ErrorTypes.TimeoutError => "The operation timed out. It took longer than expected.",
                _ => "An unknown error occurred.",
            };
        }
    }
}
