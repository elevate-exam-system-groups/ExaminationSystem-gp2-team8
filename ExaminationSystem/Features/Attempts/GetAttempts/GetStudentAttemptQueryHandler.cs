using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExaminationSystem.Features.Attempts.GetAttempts
{
    public class GetStudentAttemptQueryHandler : IRequestHandler<GetStudentAttemptQuery, PaginatedResult<QuizHistoryDto>>
    {
        private readonly IGeneralRepository<Domain.Entities.Attempts> _repository;

        public GetStudentAttemptQueryHandler(IGeneralRepository<Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public async Task<PaginatedResult<QuizHistoryDto>> Handle(GetStudentAttemptQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query()
                .AsNoTracking()
                .Where(a => a.UserId == request.StudentId && a.Attempt != Domain.Enums.AttemptStatus.InProgress);

            if (request.quizId.HasValue)
                query = query.Where(a=> a.QuizId == request.quizId);

            if (request.diplomaId.HasValue)
                query = query.Where(a=> a.Quiz.DiplomaId == request.diplomaId);

            query = query.OrderByDescending(a => a.SubmittedAt);

            var count = await query.CountAsync(cancellationToken);

            var items = await query.Skip((request.page -1) * request.perPage)
                .Take(request.perPage)
                .Select(a => new QuizHistoryDto(
                    a.Id,
                    a.Quiz.Title,
                    a.score,
                    a.Attempt.ToString(),
                    a.score >= a.Quiz.PassScore,
                    a.SubmittedAt
                    )).ToListAsync(cancellationToken);

            return new PaginatedResult<QuizHistoryDto>(items, count, request.page, request.perPage);
        }
    }
}
