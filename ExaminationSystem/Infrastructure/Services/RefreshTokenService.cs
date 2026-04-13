using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

namespace ExaminationSystem.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ExamAppDbContext _context;
        private const int ExpiryDays = 7;
        public RefreshTokenService(ExamAppDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateRefreshTokenAsync(int userId, string ipAddress, CancellationToken cancellationToken = default)
        {
            // ── Generate cryptographically secure raw token ────────────────────
            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var tokenHash = HashToken(rawToken);
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                TokenHash = tokenHash,
                IpAddress = ipAddress,
                Revoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(ExpiryDays),
                CreatedAt = DateTime.UtcNow,
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            // Return raw token — this is the ONLY time it exists in plain text
            return rawToken;
        }

        public Task RevokeAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        // ── Helper ────────────────────────────────────────────────────────────────
        public static string HashToken(string rawToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToBase64String(bytes);
        }
    }

}
