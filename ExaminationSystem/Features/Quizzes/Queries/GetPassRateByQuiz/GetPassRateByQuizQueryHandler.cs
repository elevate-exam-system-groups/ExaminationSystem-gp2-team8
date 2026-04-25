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
            var query = _repository.Query()
                       .AsNoTracking();


            if (request.Filters.DiplomaId.HasValue)
            {
                query = query.Where(q => q.DiplomaId == request.Filters.DiplomaId.Value);
            }
            if (request.Filters.From.HasValue)
            {
                query = query.Where(q =>
                    q.Attempts.Any(a => a.SubmittedAt >= request.Filters.From.Value));
            }

            if (request.Filters.To.HasValue)
            {
                query = query.Where(q =>
                    q.Attempts.Any(a => a.SubmittedAt <= request.Filters.To.Value));
            }
            var quizzes = await query.Select(x => new PassRateByQuizDTO
            {
                QuizId = x.Id,
                QuizTitle = x.Title,
                TotalAttempts = x.Attempts.Count(),
                PassedAttempts = x.Attempts.Count(a => a.score >= x.PassScore),
                PassRate = x.Attempts.Count() == 0
                 ? 0
                 : (double)x.Attempts.Count(a => a.score >= x.PassScore)
                   * 100 / x.Attempts.Count()
            })
            .ToListAsync(cancellationToken);


            return quizzes;
        }
    }
}
