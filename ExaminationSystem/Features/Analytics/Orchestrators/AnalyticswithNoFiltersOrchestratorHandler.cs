using ExaminationSystem.Features.Analytics.DTO;
using ExaminationSystem.Features.AnswerQuestion.GetTopFailedQuestion;
using ExaminationSystem.Features.Attempts.GetAttemptsOverTime;
using ExaminationSystem.Features.Diplomas.Queries.GetAvergeScorePerDiploma;
using ExaminationSystem.Features.Quizzes.Queries.GetPassRateByQuiz;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.Analytics.Orchestrators
{
    public class AnalyticswithNoFiltersOrchestratorHandler : IRequestHandler<AnalyticswithNoFiltersOrchestrator, AnalysticsWithNoFilterDTO>
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        public AnalyticswithNoFiltersOrchestratorHandler(IMediator mediator, IMemoryCache cache)
        {
          _mediator = mediator;
          _cache = cache;
        }
        public async Task<AnalysticsWithNoFilterDTO> Handle(AnalyticswithNoFiltersOrchestrator request, CancellationToken cancellationToken)
        {
            var cacheKey = $"analytics" +
                   $"_d{request.Filters.DiplomaId}" +
                   $"_from{request.Filters.From:yyyyMMdd}" +
                   $"_to{request.Filters.To:yyyyMMdd}";

            // Return cached result if available
            if (_cache.TryGetValue(cacheKey, out AnalysticsWithNoFilterDTO? cached))
                return cached!;




            var quizzes_per_rate=await _mediator.Send(new GetPassRateByQuizQuery(request.Filters), cancellationToken);

            var avg_score_by_diploma=await _mediator.Send(new GetAverageScorePerDiploamQuery(request.Filters), cancellationToken);

            var attempts_over_time=await _mediator.Send(new GetAttemptsOverTimeQuery(request.Filters), cancellationToken);

            // top_failed_questions
            var top_failed_questions = await _mediator.Send(new GetTopFailedQuestionQuery(request.Filters), cancellationToken);    


            var result= new AnalysticsWithNoFilterDTO()
            {
                pass_rate_by_quiz = quizzes_per_rate,
                Avg_score_by_diploma = avg_score_by_diploma,
                attempts_over_time = attempts_over_time,
                top_failed_questions = top_failed_questions
            };

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            });

            return result;
           
        }
    }
}
