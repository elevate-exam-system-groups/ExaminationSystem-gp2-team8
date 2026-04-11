using Microsoft.OpenApi.Models;

namespace ExaminationSystem.BuildingBlocks.ExceptionHandling
{
    public class ApiResponse<T>
    {
        public bool isSuccess { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }

        public static ApiResponse<T> SuccessResponse(T data) => new() { isSuccess = true, Data = data };
        public static ApiResponse<T> FailureResponse(string message, string? code = null, object? details = null)
            => new() { isSuccess = false, Error = new() { Message = message, Code = code, Details = details} };

    }

    public class ApiError
    {
        public string? Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Details { get; set; }
    }
}
