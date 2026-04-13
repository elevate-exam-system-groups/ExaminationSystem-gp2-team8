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


        public RegisterUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;

        }
        public async Task<ApiResponse<RegisterResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
           
            var existing = await _userManager.FindByEmailAsync(request.userDto.Email);
            if (existing is not null)
            {
                return ApiResponse<RegisterResponseDto>.Fail("An account with this email already exists.");
            }
            var user = new User()
            {
                
                FullName = request.userDto.FullName,
                Email = request.userDto.Email,
                Status = request.userDto.UserStatus
            };

            var result = await _userManager.CreateAsync(user, request.userDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                //_logger.LogWarning("Registration failed for {Email}: {Errors}", request.userDto.Email, errors);
                return ApiResponse<RegisterResponseDto>.Fail("Registration failed.",errors);
            }
            // assign user to role
            
            await _userManager.AddToRoleAsync(user, "Student");


            var response = new RegisterResponseDto
            {
                UserId=user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Message = "Account created.",
            };

            return ApiResponse<RegisterResponseDto>.Ok(response);
        }
    }
}
