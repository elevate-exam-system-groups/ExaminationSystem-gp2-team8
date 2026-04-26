using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Users.RegisterUser
{
    public record RegisterUserCommand(UserForRegisterationDto userDto) : IRequest<RegisterResponseDto>;

    
}
