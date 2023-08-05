// Namespace holding all the application DTOs
namespace AuthFlow.Application.DTOs
{
    // OperationResult is a class used to encapsulate the result of an operation.
    // This class is generic and can hold any type of data associated with the operation result.
    public class OperationResult<T>
    {
        // A boolean indicating whether the operation was successful or not
        public bool IsSuccessful { get; set; }

        // Message related to the operation result. This could be error messages or success details.
        // This may be null if no message is set.
        public string? Message { get; set; }

        public ErrorTypes? Types { get; set; }

        // The actual data associated with the operation result. This can be any type as specified by T.
        // This may be null if no data is associated with the operation.
        public T? Data { get; set; }

        // Factory method for creating a successful operation result with the given data and message.
        // "data" parameter is the data that operation returns
        // "message" parameter is an optional success message, empty by default
        public static OperationResult<T> Success(T data, string message = "")
        {
            return new OperationResult<T> { IsSuccessful = true, Message = message, Data = data };
        }

        public static OperationResult<T> FailureBusinessValidation(string message)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message, Types = ErrorTypes.BusinessValidationError };
        }

        public static OperationResult<T> FailureDatabase(string message)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message, Types = ErrorTypes.DatabaseError };
        }

        public static OperationResult<T> FailureExtenalService(string message)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message, Types = ErrorTypes.ExternalServicesError };
        }

        public static OperationResult<T> FailureUnexpectedError(string message)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message, Types = ErrorTypes.UnexpectedError };
        }

        public static OperationResult<T> FailureDataSubmittedInvalid(string message)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message, Types = ErrorTypes.DataSubmittedInvalid };
        }

        public static OperationResult<T> FailureConfigurationMissingError(string message)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message, Types = ErrorTypes.ConfigurationMissingError };
        }
        private OperationResult()
        {
            
        }

        public OperationResult<bool> GetBool()
        { 
            if (IsSuccessful.Equals(true))
            {
                throw new Exception("This method can only be used if the value of IsSuccessful is false.");
            }

            return new OperationResult<bool> 
            { 
                IsSuccessful = false, 
                Message = this.Message, 
                Types = this.Types
            };
        }

        public OperationResult<string> GetString()
        {
            if (IsSuccessful.Equals(true))
            {
                throw new Exception("This method can only be used if the value of IsSuccessful is false.");
            }

            return new OperationResult<string>
            {
                IsSuccessful = false,
                Message = this.Message,
                Types = this.Types
            };
        }

        public OperationResult<int> GetInt()
        {
            if (IsSuccessful.Equals(true))
            {
                throw new Exception("This method can only be used if the value of IsSuccessful is false.");
            }

            return new OperationResult<int>
            {
                IsSuccessful = false,
                Message = this.Message,
                Types = this.Types
            };
        }

    }
}
