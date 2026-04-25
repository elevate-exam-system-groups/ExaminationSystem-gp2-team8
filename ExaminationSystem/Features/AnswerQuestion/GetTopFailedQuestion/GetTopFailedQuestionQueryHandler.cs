using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.AnswerQuestion.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AnswerQuestion.GetTopFailedQuestion
{
    public class GetTopFailedQuestionQueryHandler : IRequestHandler<GetTopFailedQuestionQuery, IEnumerable<TopFailedQuestionDTO>>
    {
        private readonly IGeneralRepository<StudentAnswer> _repository;

        public GetTopFailedQuestionQueryHandler(IGeneralRepository<StudentAnswer> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<TopFailedQuestionDTO>> Handle(GetTopFailedQuestionQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query().AsNoTracking();

            if(request.Filters.DiplomaId.HasValue)
                query = query.Where(x => x.Question.Quiz.DiplomaId == request.Filters.DiplomaId.Value);

            if(request.Filters.From.HasValue)
                query = query.Where(x => x.AnsweredAt >= request.Filters.From.Value);
            if(request.Filters.To.HasValue)
                query = query.Where(x => x.AnsweredAt <= request.Filters.To.Value);

            var result = await query
                .GroupBy(x => new { x.QuestionId, x.Question.QuestionText })
                .Select(g => new
                {
                    g.Key.QuestionId,
                    g.Key.QuestionText,
                    AnswerCount = g.Count(),
                    FailedCount = g.Count(x => !x.IsCorrect),
                    CorrectCount = g.Count(x => x.IsCorrect)
                })
                .Where(x => x.AnswerCount > 0 &&
                            ((double)x.CorrectCount * 100 / x.AnswerCount) < 40)
                .OrderBy(x => (double)x.CorrectCount * 100 / x.AnswerCount)
                .ThenByDescending(x => x.FailedCount)
                .Take(5)
                .Select(x => new TopFailedQuestionDTO
                {
                    QuestionId = x.QuestionId,
                    QuestionText = x.QuestionText,
                    FailedCount = x.FailedCount,
                    CorrectAnswerRate = (double)x.CorrectCount * 100 / x.AnswerCount
                })
                .ToListAsync();

            return result;
        }
    }
}
