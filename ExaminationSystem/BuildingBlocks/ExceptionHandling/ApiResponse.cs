using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace ExaminationSystem.BuildingBlocks.ExceptionHandling
{
    public record ApiResponse<T>
    {
        public bool isSuccess { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }

        public static ApiResponse<T> SuccessResponse(T data) => new() { isSuccess = true, Data = data };
        public static ApiResponse<T> FailureResponse(string message, string? code = null, object? details = null)
            => new() { isSuccess = false, Error = new() { Message = message, Code = code, Details = details} };

        // Authentication //
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        //public T? Data { get; init; }
        public List<string> Errors { get; init; } = [];
        public int StatusCode { get; init; } = 200;

        // Not serialized — carries refresh token from handler → controller only
        [JsonIgnore]
        public string? RefreshToken { get; init; }

        public static ApiResponse<T> Ok(T data, string message = "Success") =>
            new() { Success = true, Message = message, Data = data, StatusCode = 200 };

        public static ApiResponse<T> Fail(string message, List<string>? errors = null, int statusCode = 400) =>
            new() { Success = false, Message = message, Errors = errors ?? [], StatusCode = statusCode };

    }

    public class ApiError
    {
        public string? Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Details { get; set; }
    }
}
