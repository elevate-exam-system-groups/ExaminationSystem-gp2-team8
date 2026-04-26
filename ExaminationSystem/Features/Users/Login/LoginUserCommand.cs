using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.Users.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Users.Login
{
    public record LoginUserCommand(LoginRequestDto Dto, string IpAddress): IRequest<Result<LoginResponseDto>>;
}
