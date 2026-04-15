using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ExaminationSystem.Features.Auth.Commands
{
    public record ForgotPasswordCommand(string Email) : IRequest<Result<string>>;

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(
            UserManager<User> userManager,
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Result<string>.Success("If this email exists, reset link has been sent");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebUtility.UrlEncode(token);

            var resetLink = $"https://yourapp.com/reset-password?email={user.Email}&token={encodedToken}";

            _logger.LogInformation("Reset Link For {Email}: {Link}", user.Email, resetLink);

            return Result<string>.Success("Reset link has been sent", resetLink);
        }
    }
}