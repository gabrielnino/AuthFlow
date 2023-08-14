// Namespace holding all the application DTOs
namespace AuthFlow.Application.DTOs
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    /// <typeparam name="T">The type of data associated with the operation result.</typeparam>
    public class OperationResult_REVIEWED<T>
    {
        private const string InvalidOperation = "This method can only be used if the value of IsSuccessful is false.";

        /// <summary>
        /// Private constructor ensures that objects can only be created using factory methods.
        /// </summary>
        private OperationResult_REVIEWED()
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
                throw new InvalidOperationResultException_REVIEWED(OperationResult_REVIEWED<T>.InvalidOperation);
            }
        }

        /// <summary>
        /// Creates a new OperationResult with the specified generic type based on the current result.
        /// </summary>
        private OperationResult_REVIEWED<U> AsType<U>()
        {
            return new OperationResult_REVIEWED<U>
            {
                IsSuccessful = false,
                Message = this.Message,
                ErrorType = this.ErrorType
            };
        }

        /// <summary>
        /// Indicates if the operation was successful.
        /// </summary>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// Contains the data associated with the operation result.
        /// </summary>
        public T? Data { get; private set; }

        /// <summary>
        /// Provides additional details about the operation, such as error messages or success information.
        /// </summary>
        public string? Message { get; private set; }

        /// <summary>
        /// Specifies the type of error, if any, that occurred during the operation.
        /// </summary>
        private ErrorTypes_REVIEWED ErrorType { get; set; }

        /// <summary>
        /// Specifies the type of error, if any, that occurred during the operation as a string.
        /// </summary>
        public string Error => this.ErrorType.ToErrorString();

        /// <summary>
        /// Creates a successful operation result with the given data and optional message.
        /// </summary>
        /// <param name="data">Data result return</param>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult_REVIEWED<T> Success(T data, string message = "")
        {
            return new OperationResult_REVIEWED<T>
            {
                IsSuccessful = true,
                Data = data,
                Message = message,
                ErrorType = ErrorTypes_REVIEWED.None
            };
        }

        /// <summary>
        /// Creates a failed operation result with the given message and error type.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="errorTypes">The error type</param>
        /// <returns>The operation result</returns>
        private static OperationResult_REVIEWED<T> Failure(string message, ErrorTypes_REVIEWED errorTypes)
        {
            return new OperationResult_REVIEWED<T> { IsSuccessful = false, Message = message, ErrorType = errorTypes };
        }

        /// <summary>
        /// Creates a failed operation result for a business validation scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult_REVIEWED<T> FailureBusinessValidation(string message)
        {
            return Failure(message, ErrorTypes_REVIEWED.BusinessValidationError);
        }

        /// <summary>
        /// Creates a failed operation result for a database scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult_REVIEWED<T> FailureDatabase(string message)
        {
            return Failure(message, ErrorTypes_REVIEWED.DatabaseError);
        }

        /// <summary>
        /// Creates a failed operation result for a external service scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult_REVIEWED<T> FailureExtenalService(string message)
        {
            return Failure(message, ErrorTypes_REVIEWED.ExternalServicesError);
        }

        /// <summary>
        /// Creates a failed operation result for a unexpected errror scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult_REVIEWED<T> FailureUnexpectedError(string message)
        {
            return Failure(message, ErrorTypes_REVIEWED.UnexpectedError);
        }

        /// <summary>
        /// Creates a failed operation result for a data sumitted invalid scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult_REVIEWED<T> FailureDataSubmittedInvalid(string message)
        {
            return Failure(message, ErrorTypes_REVIEWED.DataSubmittedInvalid);
        }

        /// <summary>
        /// Creates a failed operation result for a configuration missing error scenario."
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The operation result</returns>
        public static OperationResult_REVIEWED<T> FailureConfigurationMissingError(string message)
        {
            return Failure(message, ErrorTypes_REVIEWED.ConfigurationMissingError);
        }

        /// <summary>
        /// Converts the current result to a boolean type.
        /// </summary>
        public OperationResult_REVIEWED<bool> ToResultWithBoolType()
        {
            EnsureIsFailure();
            return AsType<bool>();
        }

        /// <summary>
        /// Converts the current result to an integer type.
        /// </summary>
        public OperationResult_REVIEWED<int> ToResultWithIntType()
        {
            EnsureIsFailure();
            return AsType<int>();
        }

        /// <summary>
        /// Converts the current result to its generic type.
        /// </summary>
        public OperationResult_REVIEWED<T> ToResultWithGenericType()
        {
            EnsureIsFailure();
            return AsType<T>();
        }
    }
}
