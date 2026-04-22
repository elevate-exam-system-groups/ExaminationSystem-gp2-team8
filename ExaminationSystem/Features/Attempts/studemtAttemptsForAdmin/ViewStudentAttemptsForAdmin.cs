using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Attempts.studemtAttemptsForAdmin
{
    public record ViewStudentAttemptsForAdmin(int pageIndex,int pageSize) : IRequest<PaginatedResult<AllAttemptForAdminDTO>>;
   
}
