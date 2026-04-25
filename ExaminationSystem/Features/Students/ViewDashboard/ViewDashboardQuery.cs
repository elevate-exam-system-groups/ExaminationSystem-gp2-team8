using ExaminationSystem.Features.Students.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Students.ViewDashboard
{
    public record ViewDashboardQuery(int studentId) :IRequest<GetOverallStatsDto>;
}
