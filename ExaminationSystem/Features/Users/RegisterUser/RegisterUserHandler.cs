using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Users.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ApiResponse<RegisterResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(UserManager<User> userManager, ILogger<RegisterUserHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<ApiResponse<RegisterResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.userDto;

            //Check for duplicate email
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing is not null)
            {
                return ApiResponse<RegisterResponseDto>.Fail("An account with this email already exists.");
            }

            // Build the user
            var user = new User()
            {

                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim().ToLowerInvariant(),
                UserName = dto.Email.Trim().ToLowerInvariant(),
                Status = UserStatus.Pending,
                DeletedAt= DateTime.UtcNow,
                EmailConfirmed=false
            };

            //Create user — Identity hashes password
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("Registration failed for {Email}: {Errors}", dto.Email, errors);
                return ApiResponse<RegisterResponseDto>.Fail("Registration failed.", errors);
            }

            // assign user to default role student
            await _userManager.AddToRoleAsync(user, "Student");

            _logger.LogInformation("New user registered: {UserId} | {Email}", user.Id, user.Email);

            var response = new RegisterResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Message = "Account created.",
            };

            return ApiResponse<RegisterResponseDto>.Ok(response);
        }
    }
}
