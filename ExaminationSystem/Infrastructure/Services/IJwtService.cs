using ExaminationSystem.Domain.Entities;

namespace ExaminationSystem.Infrastructure.Services
{
    public interface IJwtService
    {
        /// <summary>Generates a signed JWT access token (15 min expiry).</summary>
        /// <returns>Signed token string and its expiry timestamp.</returns>
        (string Token, DateTime ExpiresAt) GenerateAccessToken(User user, IList<string> roles);
    }
}
