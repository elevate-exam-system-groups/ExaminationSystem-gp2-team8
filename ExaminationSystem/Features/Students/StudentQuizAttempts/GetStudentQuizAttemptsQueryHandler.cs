using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Students.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Students.StudentQuizAttempts
{
    public class GetStudentQuizAttemptsQueryHandler : IRequestHandler<GetStudentQuizAttemptsQuery, IEnumerable<StudentQuizAttemptsDto>>
    {
        private readonly IGeneralRepository<Domain.Entities.Attempts> _repository;
        public GetStudentQuizAttemptsQueryHandler(IGeneralRepository<Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<StudentQuizAttemptsDto>> Handle(GetStudentQuizAttemptsQuery request, CancellationToken cancellationToken)
        {
            var diplomaIds = request.DiplomasIds ?? new List<int>();

            var query = _repository.Query()
                .AsNoTracking()
                .Where(a => a.UserId == request.studentId && a.Attempt != AttemptStatus.InProgress);

            if (diplomaIds.Count > 0)
            {
                query = query.Where(a => diplomaIds.Contains(a.Quiz.DiplomaId));
            }

            var recentAttempts = await query
                .GroupBy(a => new
                {
                    DiplomaTitle = a.Quiz.Diploma.Title,
                    QuizName = a.Quiz.Title
                })
                .Select(g => new StudentQuizAttemptsDto
                {
                    DiplomaTitle = g.Key.DiplomaTitle,
                    QuizName = g.Key.QuizName,
                    Attempts = g.Count(),
                    AttemptDate = g.Max(a => a.SubmittedAt == default ? a.CreatedAt : a.SubmittedAt)
                })
                .OrderByDescending(a => a.AttemptDate)
                .ToListAsync(cancellationToken);

            return recentAttempts;
        }
    }
}
