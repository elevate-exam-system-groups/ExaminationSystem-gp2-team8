using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.AdminStats.DTOs;
using ExaminationSystem.Features.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace ExaminationSystem.Features.Users.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<RegisterResponseDto>>
    {
        private readonly UserManager<User> _userManager;

        public RegisterUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<RegisterResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.userDto;

            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing is not null)
                throw new ConflictException("An account with this email already exists.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Status = UserStatus.Pending
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException("Registration failed.", errors);
            }

            await _userManager.AddToRoleAsync(user, "Student");

            var registerResponseDto = new RegisterResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Message = "Account created.",
            };
            if (registerResponseDto is null)
                return Result<RegisterResponseDto>.Failure("No data found.", 404);
            return Result<RegisterResponseDto>.Success(registerResponseDto);
        }
    }
}