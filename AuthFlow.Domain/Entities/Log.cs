namespace AuthFlow.Domain.Entities
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    ///   The Log class represents a log entry in the system.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Message property contains the main information about the log entry.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// EntityName property contains the name of the entity that was involved in the event.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// EntityValue property contains a serialized version of the entity's state at the moment the event occurred.
        /// </summary>
        public string EntityValue { get; set; }

        /// <summary>
        /// Level property represents the severity of the log entry, converted to a string for easier JSON serialization.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel Level { get; set; }

        /// <summary>
        /// Operation property indicates what operation was executed, converted to a string for easier JSON serialization.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public OperationExecute Operation { get; set; }

        /// <summary>
        /// CreatedAt property stores the date and time when the log entry was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// LogLevel is an enumeration of possible log levels. The level of a log entry indicates its severity.
    /// </summary>
    public enum LogLevel
    {
        Trace,       // Used for the most detailed log outputs.
        Debug,       // Used for interactive investigation during development.
        Information, // Used to track the general flow of the application.
        Warning,     // Used for logs that highlight the abnormal or unexpected events in the application flow.
        Error,       // Used for logs that highlight when the current flow of execution is stopped due to a failure.
        Fatal        // Used to log unhandled exceptions which forces the program to crash.
    }

    // OperationExecute is an enumeration of possible operations that can be performed on an entity.
    public enum OperationExecute
    {
        // Crud
        Add,
        Modified,
        Remove,
        Deactivate,
        Activate,
        // Others
        GetUserById,
        GetAllByFilter,
        GetPageByFilter,
        GetCountFilter,
        GenerateOtp,
        LoginOtp,
        Login,
        Validate,
        ValidateEmail,
        ValidateOtp,
        ValidateUsername,
        SetNewPassword,
        SendEmailAsync
    }
}