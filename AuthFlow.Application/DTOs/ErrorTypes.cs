namespace AuthFlow.Application.DTOs
{
    public enum ErrorTypes
    {
        DatabaseError,
        BusinessValidationError,
        NetworkError,
        UserInputError,
        NotFoundError,
        AuthenticationError,
        AuthorizationError,
        ResourceError,
        TimeoutError,
        UnexpectedError
    }

    public static class ErrorTypesExtensions
    {
        public static string GetString(this ErrorTypes error)
        {
            switch (error)
            {
                case ErrorTypes.DatabaseError:
                    return "DATABASE_ERROR";
                case ErrorTypes.BusinessValidationError:
                    return "BUSINESS_VALIDATION_ERROR";
                case ErrorTypes.NetworkError:
                    return "NETWORK_ERROR";
                case ErrorTypes.UserInputError:
                    return "USER_INPUT_ERROR";
                case ErrorTypes.NotFoundError:
                    return "NOT_FOUND_ERROR";
                case ErrorTypes.AuthenticationError:
                    return "AUTHENTICATION_ERROR";
                case ErrorTypes.AuthorizationError:
                    return "AUTHORIZATION_ERROR";
                case ErrorTypes.ResourceError:
                    return "RESOURCE_ERROR";
                case ErrorTypes.TimeoutError:
                    return "TIMEOUT_ERROR";
                case ErrorTypes.UnexpectedError:
                    return "UNEXPECTED_ERROR";
                default:
                    return "UNKNOWN_ERROR";
            }
        }
    }

}
