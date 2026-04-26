using System.Text.Json.Serialization;

namespace ExaminationSystem.Domain.Common
{
    public record Result<T>
    {
        //public bool IsSuccess { get; set; }
        //public string? Message { get; set; }
        //public List<string> Errors { get; set; } = new();

        //public static Result Success(string message = "Success") 
        //    => new Result { IsSuccess = true, Message = message };  

        //public static Result Fail(string message, List<string>? errors = null)
        //    => new Result { IsSuccess = false, Message = message, Errors = errors ?? new List<string>() };

        /// <summary>
        /// Edited
        /// </summary>
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; private set; }
        public string Error { get; private set; } = string.Empty;
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public string? RefreshToken { get; init; }
        // Private constructor — force use of factory methods
        private Result() { }

        public static Result<T> Success(T value) =>
            new() { IsSuccess = true, Value = value, StatusCode = 200 };

        public static Result<T> Failure(string error, int statusCode = 400) =>
            new() { IsSuccess = false, Error = error, StatusCode = statusCode };
    }
} 
