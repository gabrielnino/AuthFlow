namespace AuthFlow.Application.DTOs
{
    public class OperationResult<T>
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static OperationResult<T> Success(T data, string message = "")
        {
            return new OperationResult<T> { IsSuccessful = true, Message = message, Data = data };
        }

        public static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T> { IsSuccessful = false, Message = message };
        }
    }
}
