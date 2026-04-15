using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Verification.Commands
{
    public record VerifyAccountCommand(string Email, string Code) : IRequest<Result<string>>;

    public class VerifyAccountCommandHandler : IRequestHandler<VerifyAccountCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public VerifyAccountCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return Result<string>.Fail("User not found");
            }

            if (user.Status == UserStatus.Active)
                return Result<string>.Fail("Account already verified");

            if (user.OtpExpiry == null || user.OtpExpiry < DateTime.UtcNow)
                return Result<string>.Fail("Code expired or not found");

            if (string.IsNullOrEmpty(user.OtpHash))
                return Result<string>.Fail("No verification code found");

            var hashedCode = HashOtp(request.Code);

            if (hashedCode != user.OtpHash)
                return Result<string>.Fail("Invalid code");

            user.Status = UserStatus.Active;
            user.OtpHash = null;
            user.OtpExpiry = null;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return Result<string>.Fail(
                    "Failed to update user",
                    updateResult.Errors.Select(e => e.Description).ToList()
                );

            return Result<string>.Success("Account verified successfully");
        }

        private static string HashOtp(string otp)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(otp);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}