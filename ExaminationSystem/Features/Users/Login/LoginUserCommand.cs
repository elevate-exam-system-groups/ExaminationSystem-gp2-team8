using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Users.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Users.Login
{
    public record LoginUserCommand(LoginRequestDto Dto, string IpAddress): IRequest<ApiResponse<LoginResponseDto>>;
}
