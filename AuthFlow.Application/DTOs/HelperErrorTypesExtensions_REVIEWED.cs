namespace AuthFlow.Application.DTOs
{
    /// <summary>
    /// Provides extension methods for the ErrorTypes enum.
    /// </summary>
    public static class HelperErrorTypesExtensions_REVIEWED
    {
        /// <summary>
        /// Gets the string representation of the specified error type.
        /// </summary>
        /// <param name="error">The error type.</param>
        /// <returns>The string representation of the error type.</returns>
        public static string ToErrorString(this ErrorTypes_REVIEWED error)
        {
            return error switch
            {
                ErrorTypes_REVIEWED.None => "NONE",
                ErrorTypes_REVIEWED.BusinessValidationError => "BUSINESS_VALIDATION_ERROR",
                ErrorTypes_REVIEWED.DatabaseError => "DATABASE_ERROR",
                ErrorTypes_REVIEWED.ExternalServicesError => "EXTERNAL_SERVICE_ERROR",
                ErrorTypes_REVIEWED.UnexpectedError => "UNEXPECTED_ERROR",
                ErrorTypes_REVIEWED.DataSubmittedInvalid => "DATA_SUBMITTED_INVALID",
                ErrorTypes_REVIEWED.ConfigurationMissingError => "CONFIGURATION_MISSING_ERROR",
                
                ErrorTypes_REVIEWED.NetworkError => "NETWORK_ERROR",
                ErrorTypes_REVIEWED.UserInputError => "USER_INPUT_ERROR",
                ErrorTypes_REVIEWED.NotFoundError => "NOT_FOUND_ERROR",
                ErrorTypes_REVIEWED.AuthenticationError => "AUTHENTICATION_ERROR",
                ErrorTypes_REVIEWED.AuthorizationError => "AUTHORIZATION_ERROR",
                ErrorTypes_REVIEWED.ResourceError => "RESOURCE_ERROR",
                ErrorTypes_REVIEWED.TimeoutError => "TIMEOUT_ERROR",
                _ => "UNKNOWN_ERROR",
            };
        }

        /// <summary>
        /// Gets a user-friendly description for the specified error type.
        /// </summary>
        /// <param name="error">The error type.</param>
        /// <returns>The description of the error type.</returns>
        public static string GetDescription(this ErrorTypes_REVIEWED error)
        {
            return error switch
            {
                ErrorTypes_REVIEWED.None => "No error occurred.",
                ErrorTypes_REVIEWED.BusinessValidationError => "A business validation error occurred.",
                ErrorTypes_REVIEWED.DatabaseError => "An error occurred while accessing the database.",
                ErrorTypes_REVIEWED.ExternalServicesError => "An error occurred with an external service.",
                ErrorTypes_REVIEWED.UnexpectedError => "An unexpected error occurred.",
                ErrorTypes_REVIEWED.DataSubmittedInvalid => "The data submitted was invalid or in an incorrect format.",
                ErrorTypes_REVIEWED.ConfigurationMissingError => "Required configuration data is missing.",

                ErrorTypes_REVIEWED.NetworkError => "An error occurred due to network issues.",
                ErrorTypes_REVIEWED.UserInputError => "There was an error with the user's input.",
                ErrorTypes_REVIEWED.NotFoundError => "The requested resource was not found.",
                ErrorTypes_REVIEWED.AuthenticationError => "An authentication error occurred. User credentials might be incorrect or expired.",
                ErrorTypes_REVIEWED.AuthorizationError => "The user does not have the necessary permissions to perform the requested operation.",
                ErrorTypes_REVIEWED.ResourceError => "There was an error related to resource allocation or access.",
                ErrorTypes_REVIEWED.TimeoutError => "The operation timed out. It took longer than expected.",
                _ => "An unknown error occurred.",
            };
        }
    }
}
