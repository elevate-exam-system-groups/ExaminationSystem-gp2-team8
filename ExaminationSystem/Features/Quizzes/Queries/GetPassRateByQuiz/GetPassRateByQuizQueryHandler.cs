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

            var quizzes=await _repository.Query()
                .AsNoTracking()
                 .Select(q => new PassRateByQuizDTO
                 {
                     QuizId = q.Id,
                     QuizTitle = q.Title,
                     TotalAttempts = q.Attempts.Count(),
                     PassedAttempts = q.Attempts.Count(a => a.score >= q.PassScore),
                     PassRate = q.Attempts.Count() == 0
                        ? 0
                        : (double)q.Attempts.Count(a => a.score >= q.PassScore) * 100 / q.Attempts.Count()
                 })
                .ToListAsync(cancellationToken);


            return  quizzes;
        }
    }
}
