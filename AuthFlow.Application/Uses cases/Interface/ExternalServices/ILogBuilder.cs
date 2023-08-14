namespace AuthFlow.Application.Uses_cases.Interface.ExternalServices
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Domain.Entities;

    public interface ILogBuilder<T>
    {
        OperationResult_REVIEWED<T> Trace(string message, object entity, OperationExecute operation);
        OperationResult_REVIEWED<T> Debug(string message, object entity, OperationExecute operation);
        OperationResult_REVIEWED<T> Information(string message, object entity, OperationExecute operation);
        OperationResult_REVIEWED<T> Warning(string message, object entity, OperationExecute operation);
        OperationResult_REVIEWED<T> Error(string message, object entity, OperationExecute operation);
        OperationResult_REVIEWED<T> Fatal(string message, object entity, OperationExecute operation);
    }
}
