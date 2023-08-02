namespace AuthFlow.Application.Uses_cases.Interface.ExternalServices
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Domain.Entities;

    public interface ILogBuilder<T>
    {
        OperationResult<T> Trace(string message, object entity, OperationExecute operation);
        OperationResult<T> Debug(string message, object entity, OperationExecute operation);
        OperationResult<T> Information(string message, object entity, OperationExecute operation);
        OperationResult<T> Warning(string message, object entity, OperationExecute operation);
        OperationResult<T> Error(string message, object entity, OperationExecute operation);
        OperationResult<T> Fatal(string message, object entity, OperationExecute operation);
    }
}
