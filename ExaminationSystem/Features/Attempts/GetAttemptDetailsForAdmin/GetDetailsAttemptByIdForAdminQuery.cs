using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Attempts.GetAttemptDetailsForAdmin
{
    public record GetDetailsAttemptByIdForAdminQuery(int Id) : IRequest<AttemptSummaryDTO?>;
   
}
