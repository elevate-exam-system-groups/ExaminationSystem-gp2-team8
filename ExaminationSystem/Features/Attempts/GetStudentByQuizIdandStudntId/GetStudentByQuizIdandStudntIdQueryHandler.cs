using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExaminationSystem.Features.Attempts.GetStudentByQuizIdandStudntId
{
    public class GetStudentByQuizIdandStudntIdQueryHandler : IRequestHandler<GetStudentByQuizIdandStudntIdQuery, PaginatedResult<FilteredAttemptsDTO>>
    {
        private readonly IGeneralRepository<Domain.Entities.Attempts> _repository;

        public GetStudentByQuizIdandStudntIdQueryHandler(IGeneralRepository<Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }


        public async Task<PaginatedResult<FilteredAttemptsDTO>> Handle(GetStudentByQuizIdandStudntIdQuery request, CancellationToken cancellationToken)
        {
           
            var attempts = _repository.Query()
                .IgnoreQueryFilters()
                            .AsNoTracking();
            if (request.quizId.HasValue)
            {
                attempts = attempts.Where(a => a.QuizId == request.quizId.Value);
            }
            if (request.studentId.HasValue)
            {
                attempts = attempts.Where(a => a.UserId == request.studentId.Value);
            }

            var attemptDTO = attempts.Select(a => new FilteredAttemptsDTO()
            {
                Id=a.Id,
                QuizId=a.QuizId,
                score=a.score,
                studentId=a.UserId,
                status=a.Attempt.ToString(),
            });
            return await attemptDTO.ApplySortandOrderBy(request.Params.SortBy, request.Params.Order).ApplyPagination(request.Params.Page,request.Params.PerPage);
            
        }
    }
}
