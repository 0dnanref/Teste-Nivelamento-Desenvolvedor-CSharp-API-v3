namespace Questao5.Domain.Entities
{
    public class CustomResult<T>
    {
        public bool Success { get; init; }
        public string Message { get; init; }
        public T? Data { get; init; }
        public string? ErrorType { get; init; }

        public CustomResult(string message = "", bool success = true, T? data = default, string? errorType = null)
        {
            Message = message;
            Success = success;
            Data = data;
            ErrorType = errorType;
        }

        public static CustomResult<T> Result(T data, string message = "") =>
            new(message, true, data);

       
    }
}
