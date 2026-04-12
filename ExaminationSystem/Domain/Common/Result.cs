namespace ExaminationSystem.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();

        public static Result Success(string message = "Success") 
            => new Result { IsSuccess = true, Message = message };  

        public static Result Fail(string message, List<string>? errors = null)
            => new Result { IsSuccess = false, Message = message, Errors = errors ?? new List<string>() };
    }
} 
