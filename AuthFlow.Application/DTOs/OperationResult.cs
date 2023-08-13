// Namespace holding all the application DTOs
namespace AuthFlow.Application.DTOs
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    /// <typeparam name="T">The type of data associated with the operation result.</typeparam>
    public class OperationResult<T>
    {
        private const string InvalidOperation = "This method can only be used if the value of IsSuccessful is false.";

        // Private constructor ensures that objects can only be created using factory methods.
        private OperationResult()
        {

        }

        /// <summary>
        /// Checks if the current operation result indicates a failure.
        /// Throws an exception if the operation was successful.
        /// </summary>
        private void EnsureIsFailure()
        {
            if (IsSuccessful.Equals(true))
            {
                throw new InvalidOperationResultException(OperationResult<T>.InvalidOperation);
            }
        }

        /// <summary>
        /// Creates a new OperationResult with the specified generic type based on the current result.
        /// </summary>
        private OperationResult<U> AsType<U>()
        {
            return new OperationResult<U>
            {
                IsSuccessful = false,
                Message = this.Message,
                Types = this.Types
            };
        }

        // Indicates if the operation was successful.
        public bool IsSuccessful { get; private set; }

        // Provides additional details about the operation, such as error messages or success information.
        public string? Message { get; private set; }

        // Specifies the type of error, if any, that occurred during the operation.
        public ErrorTypes? Types { get; private set; }

        // Contains the data associated with the operation result.
        public T? Data { get; private set; }

        /// <summary>
        /// Creates a successful operation result with the given data and optional message.
        /// </summary>
        /// <param name="data">Data result return</param>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult<T> Success(T data, string message = "")
        {
            return new OperationResult<T> { IsSuccessful = true, Message = message, Data = data };
        }

        /// <summary>
        /// Creates a failed operation result with the given message and error type.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="errorTypes">The error type</param>
        /// <returns>The operation result</returns>
        private static OperationResult<T> Failure(string message, ErrorTypes errorTypes)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message, Types = errorTypes };
        }

        /// <summary>
        /// Creates a failed operation result for a business validation scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult<T> FailureBusinessValidation(string message)
        {
            return Failure(message, ErrorTypes.BusinessValidationError);
        }

        /// <summary>
        /// Creates a failed operation result for a database scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult<T> FailureDatabase(string message)
        {
            return Failure(message, ErrorTypes.DatabaseError);
        }

        /// <summary>
        /// Creates a failed operation result for a external service scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult<T> FailureExtenalService(string message)
        {
            return Failure(message, ErrorTypes.ExternalServicesError);
        }

        /// <summary>
        /// Creates a failed operation result for a unexpected errror scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult<T> FailureUnexpectedError(string message)
        {
            return Failure(message, ErrorTypes.UnexpectedError);
        }

        /// <summary>
        /// Creates a failed operation result for a data sumitted invalid scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult<T> FailureDataSubmittedInvalid(string message)
        {
            return Failure(message, ErrorTypes.DataSubmittedInvalid);
        }

        /// <summary>
        /// Creates a failed operation result for a configuration missing error scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult<T> FailureConfigurationMissingError(string message)
        {
            return Failure(message, ErrorTypes.ConfigurationMissingError);
        }

        /// <summary>
        /// Converts the current result to a boolean type.
        /// </summary>
        public OperationResult<bool> ToResultWithBoolType()
        {
            EnsureIsFailure();
            return AsType<bool>();
        }

        /// <summary>
        /// Converts the current result to an integer type.
        /// </summary>
        public OperationResult<int> ToResultWithIntType()
        {
            EnsureIsFailure();
            return AsType<int>();
        }

        /// <summary>
        /// Converts the current result to its generic type.
        /// </summary>
        public OperationResult<T> ToResultWithGenericType()
        {
            EnsureIsFailure();
            return AsType<T>();
        }
    }
}
