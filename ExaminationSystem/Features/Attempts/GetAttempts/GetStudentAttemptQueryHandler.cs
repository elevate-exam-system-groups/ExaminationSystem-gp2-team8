using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Attempts.DTOs;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExaminationSystem.Features.Attempts.GetAttempts
{
    public class GetStudentAttemptQueryHandler : IRequestHandler<GetStudentAttemptQuery, PaginatedResult<QuizHistoryDto>>
    {
        private readonly ExamAppDbContext _dbContext;

        public GetStudentAttemptQueryHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PaginatedResult<QuizHistoryDto>> Handle(GetStudentAttemptQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Attempts.Where(a => a.UserId == request.StudentId);

            if(request.quizId.HasValue)
                query = query.Where(a=> a.QuizId == request.quizId);

            if (request.diplomaId.HasValue)
                query = query.Where(a=> a.Quiz.DiplomaId == request.diplomaId);

            query = query.OrderByDescending(a => a.StartTime);

            var count = await query.CountAsync();

            var items = await query.Skip((request.page -1) * request.perPage)
                .Take(request.perPage)
                .Select(a => new QuizHistoryDto(
                    a.Id,
                    a.Quiz.Title,
                    a.score,
                    a.Attempt.ToString(),
                    a.score >= a.Quiz.PassScore,
                    a.StartTime
                    )).ToListAsync();

            return new PaginatedResult<QuizHistoryDto>(items, count, request.page, request.perPage);
        }
    }
}
