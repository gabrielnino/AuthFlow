using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AuthFlow.Domain.Entities
{
    public class Log
    {
        public string Message { get; set; }
        public string EntityName { get; set; }
        public string EntityValue { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel Level { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public OperationExecute Operation { get; set; }
        public DateTime CreatedAt { get; set; }
        

        private Log()
        {
            
        }

        public static Log Trace(string message, object entity, OperationExecute operation)
        {
            return GetLog(message, entity, LogLevel.Trace, operation);
        }

        public static Log Debug(string message, object entity, OperationExecute operation)
        {
            return GetLog(message, entity, LogLevel.Debug, operation);
        }

        public static Log Information(string message, object entity, OperationExecute operation)
        {
            return GetLog(message, entity, LogLevel.Information, operation);
        }

        public static Log Warning(string message, object entity, OperationExecute operation)
        {
            return GetLog(message, entity, LogLevel.Warning, operation);
        }

        public static Log Error(string message, object entity, OperationExecute operation)
        {
            return GetLog(message, entity, LogLevel.Error, operation);
        }

        public static Log Fatal(string message, object entity, OperationExecute operation)
        {
            return GetLog(message, entity, LogLevel.Fatal, operation);
        }


        private static Log GetLog(string message, object entity, LogLevel level, OperationExecute operation)
        {
            return new Log 
            {
                Message = message,
                EntityName = entity.GetType().Name,
                EntityValue = JsonConvert.SerializeObject(entity),
                Level = level,
                Operation = operation, 
                CreatedAt = DateTime.UtcNow 
            };
        }
    }
    public enum LogLevel
    {
        Trace = 0,    // Used for the most detailed log outputs.
        Debug = 1,    // Used for interactive investigation during development.
        Information = 2, // Used to track the general flow of the application.
        Warning = 3,  // Used for logs that highlight the abnormal or unexpected events in the application flow.
        Error = 4,    // Used for logs that highlight when the current flow of execution is stopped due to a failure.
        Fatal = 5     // Used to log unhandled exceptions which forces the program to crash.
    }

    public enum OperationExecute
    {
        GetAllByFilter,
        GetPageByFilter,
        GetCountByFilter,
        Add,
        Modified,
        Remove,
        Deactivate,
        Activate
    }

}
