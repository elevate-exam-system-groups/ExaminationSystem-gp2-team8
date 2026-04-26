using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.AdminStats.DTOs;
using MediatR;

namespace ExaminationSystem.Features.AdminStats
{
    public record GetAdminStatsQuery : IRequest<Result<AdminStatsDto>>;
    
}
