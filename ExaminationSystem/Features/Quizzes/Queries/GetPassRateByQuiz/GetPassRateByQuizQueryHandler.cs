using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Queries.GetPassRateByQuiz
{
    public class GetPassRateByQuizQueryHandler : IRequestHandler<GetPassRateByQuizQuery,
         IEnumerable<PassRateByQuizDTO>>
    {
        private readonly IGeneralRepository<Quiz> _repository;

        public GetPassRateByQuizQueryHandler(IGeneralRepository<Quiz> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<PassRateByQuizDTO>> Handle(GetPassRateByQuizQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query().AsNoTracking();
            var from = request.Filters.From;
            var to = request.Filters.To;

            if (request.Filters.DiplomaId.HasValue)
            {
                query = query.Where(q => q.DiplomaId == request.Filters.DiplomaId.Value);
            }

            var quizzes = await query
                .Select(x => new PassRateByQuizDTO
                {
                    QuizId = x.Id,
                    QuizTitle = x.Title,
                    TotalAttempts = x.Attempts.Count(a =>
                        (!from.HasValue || a.SubmittedAt >= from.Value) &&
                        (!to.HasValue || a.SubmittedAt <= to.Value)),
                    PassedAttempts = x.Attempts.Count(a =>
                        (!from.HasValue || a.SubmittedAt >= from.Value) &&
                        (!to.HasValue || a.SubmittedAt <= to.Value) &&
                        a.score >= x.PassScore)
                })
                .Where(x => x.TotalAttempts > 0)
                .ToListAsync();

            foreach (var quiz in quizzes)
            {
                quiz.PassRate = quiz.TotalAttempts == 0
                    ? 0
                    : (double)quiz.PassedAttempts * 100 / quiz.TotalAttempts;
            }

            return quizzes;
        }
    }
}
