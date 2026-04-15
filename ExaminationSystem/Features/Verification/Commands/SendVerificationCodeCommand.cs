using ExaminationSystem.API.Middlewares;
using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Cryptography;

namespace ExaminationSystem.Features.Verification.Commands
{
    public record SendVerificationCodeCommand(string Email) : IRequest<Result<string>>;

    public class SendVerificationCodeCommandHandler : IRequestHandler<SendVerificationCodeCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<SendVerificationCodeCommandHandler> _logger;
        private readonly IEmailSender _emailSender;

        public SendVerificationCodeCommandHandler(UserManager<User> userManager,
            ILogger<SendVerificationCodeCommandHandler> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<Result<string>> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Result<string>.Fail("User not found");

            var otp = GenerateOtp();

            user.OtpHash = HashOtp(otp);
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(15);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to update user {Email} with OTP", request.Email);
                return Result<string>.Fail("Failed to send verification code");
            }

            try
            {
                await _emailSender.SendEmailAsync(
                    user.Email!,
                    "Verification Code",
                    $"Your verification code is: {otp}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", request.Email);
                return Result<string>.Fail("Failed to send email");
            }

            _logger.LogInformation("Verification code sent to {Email}", request.Email);

            return Result<string>.Success("Verification code sent successfully");
        }

        private static string GenerateOtp()
        {
            return RandomNumberGenerator.GetInt32(100000, 999999).ToString();
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