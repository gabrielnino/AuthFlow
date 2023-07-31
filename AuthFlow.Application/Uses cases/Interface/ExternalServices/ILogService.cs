// The namespace AuthFlow.Application.Use_cases.Interface.ExternalServices contains interfaces for external service integrations.
// This is part of the application layer in the Clean Architecture approach and is used to define contracts or services needed by the application, 
// but implemented externally.
using AuthFlow.Application.DTOs;
using AuthFlow.Domain.Entities;

namespace AuthFlow.Application.Use_cases.Interface.ExternalServices
{
    // The ILogService interface defines the contract for a logging service.
    // The logging service is responsible for recording the events or actions that occur within the application.
    // By following this interface, different implementations can be swapped in and out as needed,
    // making the application more flexible and maintainable.
    public interface ILogService
    {
        // The CreateLog method takes in a log object, which encapsulates the details of an event that occurred in the system.
        // The implementation of this method should handle the actual logging of the event, 
        // for example, writing it to a database or sending it to an external logging service.
        Task<OperationResult<string>> CreateLog(Log log);
    }
}
