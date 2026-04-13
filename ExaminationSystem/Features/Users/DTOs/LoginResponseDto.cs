namespace ExaminationSystem.Features.Users.DTOs
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; set; }

        // Refresh token is returned in an HttpOnly cookie — NOT in this body
        // This field is only for documentation purposes
        public string TokenType { get; set; } = "Bearer";

        public UserInfoDto User { get; set; } = null!;
    }
}
