using System.Text.Json.Serialization;

namespace ExaminationSystem.BuildingBlocks.ExceptionHandling
{
    public record ApiResponse<T>
    {
        public bool IsSuccess { get; init; }
        public T? Data { get; init; }
        public string Message { get; init; } = string.Empty;
        public int StatusCode { get; init; } = 200;
        public List<string> Errors { get; init; } = [];
        public ApiError? Error { get; init; }
        public object? Meta { get; init; }

        [JsonIgnore]
        public string? RefreshToken { get; init; }


        public static ApiResponse<T> Ok(T data, string message = "Success") =>
            new() { IsSuccess = true, Data = data, Message = message, StatusCode = 200 };

        public static ApiResponse<T> Created(T data, string message = "Created") =>
            new() { IsSuccess = true, Data = data, Message = message, StatusCode = 201 };

 

        public static ApiResponse<T> Fail(
            string message,
            string? code = null,
            object? details = null,
            List<string>? errors = null,
            int statusCode = 400) =>
            new()
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode,
                Errors = errors ?? [],
                Error = (code is not null || details is not null)
                    ? new ApiError { Code = code, Message = message, Details = details }
                    : null
            };


        public static ApiResponse<T> Unauthorized(string message = "Unauthorized") =>
            Fail(message, code: "401", statusCode: 401);

        public static ApiResponse<T> Forbidden(string message = "Forbidden") =>
            Fail(message, code: "403", statusCode: 403);

        public static ApiResponse<T> NotFound(string message = "Not found") =>
            Fail(message, code: "404", statusCode: 404);

        public static ApiResponse<T> Conflict(string message = "Conflict") =>
            Fail(message, code: "409", statusCode: 409);
    }

    public class ApiError
    {
        public string? Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Details { get; set; }
    }
}