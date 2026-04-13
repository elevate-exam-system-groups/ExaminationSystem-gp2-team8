namespace ExaminationSystem.Infrastructure.Services
{
    public interface IRefreshTokenService
    {
        /// <summary>Creates a new refresh token, persists it, and returns the raw value
        /// to be written into an HttpOnly cookie.</summary>
        Task<string> CreateRefreshTokenAsync(int userId, string ipAddress,
            CancellationToken cancellationToken = default);

        /// <summary>Revokes all active refresh tokens for the given user (e.g. on password reset).</summary>
        Task RevokeAllAsync(int userId, CancellationToken cancellationToken = default);
    }
}
