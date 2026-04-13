namespace ExaminationSystem.Features.Users.DTOs
{
    public class RegisterResponseDto
    {
        public int UserId { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
