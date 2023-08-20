/// <summary>
/// Provides utility functions to support the infrastructure layer.
/// </summary>
namespace AuthFlow.Infraestructure.Other
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Infraestructure.ExternalServices;
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Utility class with static helper methods.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Creates a log entry based on the provided exception.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="entity">The related entity for which the exception occurred.</param>
        /// <param name="operation">Details of the operation being executed when the exception occurred.</param>
        /// <returns>A log entry with details of the exception.</returns>
        /// <exception cref="Exception">Thrown when the provided exception is null.</exception>
        public static Log GetLogError(Exception ex, object entity, OperationExecute operation)
        {
            if (ex == null)
            {
                throw new Exception(Resource.FailedLogValidException);
            }

            // Prepare the message for the log entry
            var message = $"Error Message: {ex.Message}  StackTrace: {ex.StackTrace}";
            var logBuilder = LogBuilder.GetLogBuilder();
            // Create the log entry
            var result = logBuilder.Error(message, entity, operation);
            // Return the log entry
            return result.Data;
        }

        /// <summary>
        /// Retrieves the name of the method or property that called this method.
        /// </summary>
        /// <param name="memberName">Name of the calling method or property. This parameter is auto-filled by the runtime.</param>
        /// <returns>The name of the calling method or property.</returns>
        public static string GetMethodName([CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}
