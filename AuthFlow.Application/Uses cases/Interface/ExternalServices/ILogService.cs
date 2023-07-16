using AuthFlow.Domain.Entities;

namespace AuthFlow.Application.Uses_cases.Interface.ExternalServices
{
    public interface ILogService
    {
        Task CreateLog(Log log);
    }
}
