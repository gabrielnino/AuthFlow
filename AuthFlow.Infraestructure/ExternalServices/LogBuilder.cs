namespace AuthFlow.Infraestructure.ExternalServices
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Uses_cases.Interface.ExternalServices;
    using AuthFlow.Domain.Entities;
    using Newtonsoft.Json;

    public class LogBuilder : ILogBuilder<Log>
    {
        private LogBuilder()
        {
            
        }

        public static LogBuilder GetLogBuilder()
        {
            return new LogBuilder();
        }

        private static OperationResult_REVIEWED<Log> ValitedTrace(string message, object entity, LogLevel level, OperationExecute operation)
        {
            try
            {
                if (message == null || entity == null)
                {
                    return OperationResult_REVIEWED<Log>.FailureDataSubmittedInvalid(Resource.FailedLogBuilderDataNotExist);
                }

                var entityName = entity.GetType().Name;
                var entityValue = JsonConvert.SerializeObject(entity);
                var log = GetLog(message, entityName, entityValue, level, operation);
                return OperationResult_REVIEWED<Log>.Success(log, Resource.SuccessfullyValidationOperationResult);
            }
            catch
            {
                return OperationResult_REVIEWED<Log>.FailureUnexpectedError(Resource.FailedValidationLogUnknowled);
            }
        }

        public OperationResult_REVIEWED<Log> Trace(string message, object entity, OperationExecute operation)
        {
            var result = ValitedTrace(message, entity, LogLevel.Trace, operation);
            if(!result.IsSuccessful)
            {
                return OperationResult_REVIEWED<Log>.FailureDataSubmittedInvalid(result.Message);
            }

            return result;
        }

        public OperationResult_REVIEWED<Log> Debug(string message, object entity, OperationExecute operation)
        {
            var result = ValitedTrace(message, entity, LogLevel.Debug, operation);
            if (!result.IsSuccessful)
            {
                return OperationResult_REVIEWED<Log>.FailureDataSubmittedInvalid(result.Message);
            }

            return result;
        }

        public OperationResult_REVIEWED<Log> Information(string message, object entity, OperationExecute operation)
        {
            var result = ValitedTrace(message, entity, LogLevel.Information, operation);
            if (!result.IsSuccessful)
            {
                return OperationResult_REVIEWED<Log>.FailureDataSubmittedInvalid(result.Message);
            }

            return result;
        }

        public OperationResult_REVIEWED<Log> Warning(string message, object entity, OperationExecute operation)
        {
            var result = ValitedTrace(message, entity, LogLevel.Warning, operation);
            if (!result.IsSuccessful)
            {
                return OperationResult_REVIEWED<Log>.FailureDataSubmittedInvalid(result.Message);
            }

            return result;
        }

        public OperationResult_REVIEWED<Log> Error(string message, object entity, OperationExecute operation)
        {
            var result = ValitedTrace(message, entity, LogLevel.Error, operation);
            if (!result.IsSuccessful)
            {
                return OperationResult_REVIEWED<Log>.FailureDataSubmittedInvalid(result.Message);
            }

            return result;
        }

        public OperationResult_REVIEWED<Log> Fatal(string message, object entity, OperationExecute operation)
        {
            var result = ValitedTrace(message, entity, LogLevel.Fatal, operation);
            if (!result.IsSuccessful)
            {
                return OperationResult_REVIEWED<Log>.FailureDataSubmittedInvalid(result.Message);
            }

            return result;
        }

        // GetLog is a private method used by the static methods to create a new Log instance. It sets
        // all the properties of the Log instance based on the parameters it receives.
        private static Log GetLog(string message, string entityName, string entityValue, LogLevel level, OperationExecute operation)
        {
            return new Log
            {
                Message = message,
                EntityName = entityName,
                EntityValue = entityValue,
                Level = level,
                Operation = operation,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
