using AuthFlow.Domain.Entities;

namespace AuthFlow.Application.Uses_cases.Interface
{
    public interface IExternalLogService
    {
        Task CreateLog(Log log);
    }
}
