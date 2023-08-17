// Define a namespace for use cases related to external services in the AuthFlow application.
namespace AuthFlow.Application.Uses_cases.Interface.ExternalServices
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Domain.Entities;

    // ILogBuilder provides an interface for building log entries 
    // with different log levels for a given type `T`.
    public interface ILogBuilder<T>
    {
        // Creates a trace log entry with the given message, entity, and operation details.
        OperationResult<T> Trace(string message, object entity, OperationExecute operation);

        // Creates a debug log entry with the given message, entity, and operation details.
        OperationResult<T> Debug(string message, object entity, OperationExecute operation);

        // Creates an informational log entry with the given message, entity, and operation details.
        OperationResult<T> Information(string message, object entity, OperationExecute operation);

        // Creates a warning log entry with the given message, entity, and operation details.
        OperationResult<T> Warning(string message, object entity, OperationExecute operation);

        // Creates an error log entry with the given message, entity, and operation details.
        OperationResult<T> Error(string message, object entity, OperationExecute operation);

        // Creates a fatal error log entry with the given message, entity, and operation details.
        OperationResult<T> Fatal(string message, object entity, OperationExecute operation);
    }
}
