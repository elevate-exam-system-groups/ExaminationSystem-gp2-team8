using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.AdminStats.DTOs;
using MediatR;

namespace ExaminationSystem.Features.AdminStats
{
    public record GetAdminStatsQuery : IRequest<ApiResponse<AdminStatsDto>>;
    
}
