namespace EventManager.Models.DTO
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public int? StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Value { get; set; }

        private Result(bool success, T? value, string? errorMessage, int? statusCode)
        {
            Success = success;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            Value = value;
        }

        public static Result<T> SuccessResult(T value) => new Result<T>(true, value, null, 200);
        public static Result<T> FailedResult(string error, int code) => new Result<T>(false, default, error, code);
    }
}
