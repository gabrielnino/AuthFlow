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

        public ErrorTypes? ErrorTypes { get; set; }

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

        // Factory method for creating a failed operation result with the given message.
        // "message" parameter is a failure message describing the reason of operation failure
        public static OperationResult<T> Failure(string message, ErrorTypes errorTypes)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message, ErrorTypes = errorTypes };
        }
    }
}
