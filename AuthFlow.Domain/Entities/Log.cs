namespace AuthFlow.Domain.Entities
{
    // The namespace AuthFlow.Domain.Entities contains all the entity classes used in the authentication flow.
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    // The Log class represents a log entry in the system. It stores information about a particular event 
    // that occurred in the system, such as an operation performed on an entity.
    public class Log
    {
        // Message property contains the main information about the log entry.
        public string Message { get; set; }

        // EntityName property contains the name of the entity that was involved in the event.
        public string EntityName { get; set; }

        // EntityValue property contains a serialized version of the entity's state at the moment the event occurred.
        public string EntityValue { get; set; }

        // Level property represents the severity of the log entry, converted to a string for easier JSON serialization.
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel Level { get; set; }

        // Operation property indicates what operation was executed, converted to a string for easier JSON serialization.
        [JsonConverter(typeof(StringEnumConverter))]
        public OperationExecute Operation { get; set; }

        // CreatedAt property stores the date and time when the log entry was created.
        public DateTime CreatedAt { get; set; }
    }

    // LogLevel is an enumeration of possible log levels. The level of a log entry indicates its severity.
    public enum LogLevel
    {
        Trace = 0,    // Used for the most detailed log outputs.
        Debug = 1,    // Used for interactive investigation during development.
        Information = 2, // Used to track the general flow of the application.
        Warning = 3,  // Used for logs that highlight the abnormal or unexpected events in the application flow.
        Error = 4,    // Used for logs that highlight when the current flow of execution is stopped due to a failure.
        Fatal = 5     // Used to log unhandled exceptions which forces the program to crash.
    }

    // OperationExecute is an enumeration of possible operations that can be performed on an entity.
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