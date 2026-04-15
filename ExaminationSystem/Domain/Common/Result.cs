namespace ExaminationSystem.Domain.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }

        public static Result<T> Success(T data, string message = "Success")
            => new Result<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };

        public static Result<T> Fail(string message, List<string>? errors = null)
            => new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
    }
}