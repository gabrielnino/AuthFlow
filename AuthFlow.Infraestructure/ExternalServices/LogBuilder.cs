namespace AuthFlow.Infraestructure.ExternalServices
{
    using AuthFlow.Application.DTOs;
    using AuthFlow.Application.Uses_cases.Interface.ExternalServices;
    using AuthFlow.Domain.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides functionality to build and validate log entries.
    /// </summary>
    public class LogBuilder : ILogBuilder<Log>
    {
        // Lazy instance ensures thread-safety for singleton initialization
        private static readonly Lazy<LogBuilder> _lazyInstance = new Lazy<LogBuilder>(() => new LogBuilder());

        // Private constructor ensures that LogBuilder objects can only be created within this class.
        private LogBuilder()
        {

        }

        /// <summary>
        /// Validates the logging data, creates and returns the log entry if valid.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="entity">The associated entity object to log.</param>
        /// <param name="operation">The operation being executed.</param>
        /// <param name="level">The severity level of the log.</param>
        /// <returns>Operation result containing the log entry if successful, or an error message if validation fails.</returns>
        private static OperationResult<Log> CreateLogIfValid(string message, object entity, OperationExecute operation, LogLevel level)
        {
            try
            {
                // Validation for message and entity
                if (string.IsNullOrWhiteSpace(message) || entity is null)
                {
                    return OperationResult<Log>.FailureDataSubmittedInvalid(Resource.FailedLogBuilderDataNotExist);
                }

                // Get the name of the entity and serialize its value
                var entityName = entity.GetType().Name;
                var entityValue = JsonConvert.SerializeObject(entity);

                // Build the log entry
                var log = GetLog(message, entityName, entityValue, level, operation);
                return OperationResult<Log>.Success(log, Resource.SuccessfullyValidationOperationResult);
            }
            catch (JsonSerializationException jsonEx)
            {
                // Handle exceptions related to JSON serialization
                return OperationResult<Log>.FailureDataSubmittedInvalid($"Failed to serialize entity: {jsonEx.Message}");
            }
            catch (NullReferenceException nullEx)
            {
                // Handle null reference exceptions
                return OperationResult<Log>.FailureUnexpectedError($"Null reference encountered: {nullEx.Message}");
            }
            catch (Exception ex)
            {
                // General error handling for unexpected issues
                return OperationResult<Log>.FailureUnexpectedError($"An unexpected error occurred: {ex.Message}");
            }
        }


        /// <summary>
        /// Creates a log entry with the specified parameters.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="entityName">The name of the associated entity.</param>
        /// <param name="entityValue">The serialized value of the entity.</param>
        /// <param name="level">The severity level of the log.</param>
        /// <param name="operation">The operation being executed.</param>
        /// <returns>A new Log entry.</returns>
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


        /// <summary>
        /// Retrieves the singleton instance of LogBuilder.
        /// </summary>
        /// <returns>The singleton instance of LogBuilder.</returns>
        public static LogBuilder GetLogBuilder() => _lazyInstance.Value;

        /// <summary>
        /// The following method create log entries for specific trace log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="entity">The entity</param>
        /// <param name="operation">The operation</param>
        /// <returns>The log</returns>
        public OperationResult<Log> Trace(string message, object entity, OperationExecute operation)
        {
            return CreateLogIfValid(message, entity, operation, LogLevel.Trace);
        }

        /// <summary>
        /// The following method create log entries for specific debug log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="entity">The entity</param>
        /// <param name="operation">The operation</param>
        /// <returns>The log</returns>
        public OperationResult<Log> Debug(string message, object entity, OperationExecute operation)
        {
            return CreateLogIfValid(message, entity, operation, LogLevel.Debug);
        }

        /// <summary>
        /// The following method create log entries for specific information log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="entity">The entity</param>
        /// <param name="operation">The operation</param>
        /// <returns>The log</returns>
        public OperationResult<Log> Information(string message, object entity, OperationExecute operation)
        {
            return CreateLogIfValid(message, entity, operation, LogLevel.Information);
        }

        /// <summary>
        /// The following method create log entries for specific warning log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="entity">The entity</param>
        /// <param name="operation">The operation</param>
        /// <returns>The log</returns>
        public OperationResult<Log> Warning(string message, object entity, OperationExecute operation)
        {
            return CreateLogIfValid(message, entity, operation, LogLevel.Warning);
        }

        /// <summary>
        /// The following method create log entries for specific error log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="entity">The entity</param>
        /// <param name="operation">The operation</param>
        /// <returns>The log</returns>
        public OperationResult<Log> Error(string message, object entity, OperationExecute operation)
        {
            return CreateLogIfValid(message, entity, operation, LogLevel.Error);
        }

        /// <summary>
        /// The following method create log entries for specific fatal log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="entity">The entity</param>
        /// <param name="operation">The operation</param>
        /// <returns>The log</returns>
        public OperationResult<Log> Fatal(string message, object entity, OperationExecute operation)
        {
            return CreateLogIfValid(message, entity, operation, LogLevel.Fatal);
        }
    }
}
