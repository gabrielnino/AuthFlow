namespace AuthFlow.Infraestructure.Other
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Infraestructure.ExternalServices;
    using System;
    using System.Runtime.CompilerServices;

    public static class Util
    {
        // Creates a log entry for an exception
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
            var resutl = logBuilder.Error(message, entity, operation);
            // Return the log entry
            return resutl.Data;
        }


        public static string GetMethodName([CallerMemberName] string memberName = "")
        {
            return memberName;
        }

    }
}
