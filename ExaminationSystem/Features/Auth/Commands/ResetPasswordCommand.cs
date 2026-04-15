using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.Commands
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword)
        : IRequest<Result<string>>;

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Token) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
                return Result<string>.Fail("Invalid input data");

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Result<string>.Fail("Invalid input data");

            var result = await _userManager.ResetPasswordAsync(
                user,
                request.Token,
                request.NewPassword
            );

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<string>.Fail("Reset failed", errors);
            }

            return Result<string>.Success("Password reset successfully");
        }
    }
}