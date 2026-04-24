using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.studemtAttemptsForAdmin
{
    public class ViewStudentAttemptsForAdminHandler : IRequestHandler<ViewStudentAttemptsForAdmin, PaginatedResult<AllAttemptForAdminDTO>>
    {
        private readonly IGeneralRepository<ExaminationSystem.Domain.Entities.Attempts> _repository;

        public ViewStudentAttemptsForAdminHandler(IGeneralRepository<ExaminationSystem.Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public async Task<PaginatedResult<AllAttemptForAdminDTO>> Handle(ViewStudentAttemptsForAdmin request, CancellationToken cancellationToken)
        {
            var query = _repository.Query()
                .AsNoTracking()
                .IgnoreQueryFilters()
                          .Select(a => new AllAttemptForAdminDTO()
                          {
                              attemptedId=a.Id,
                              studentId = a.UserId,
                              quiztitle = a.Quiz.Title,
                              score =a.score,
                              status=a.Attempt.ToString(),
                              submittedAt=a.SubmittedAt,
                              
                          });

            
            return await query.ApplyPagination(request.pageIndex,request.pageSize);
           
        }
    }
}
