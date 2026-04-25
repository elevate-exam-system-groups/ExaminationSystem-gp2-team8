using ExaminationSystem.Features.Analytics.DTO;
using ExaminationSystem.Features.AnswerQuestion.GetTopFailedQuestion;
using ExaminationSystem.Features.Attempts.GetAttemptsOverTime;
using ExaminationSystem.Features.Diplomas.Queries.GetAvergeScorePerDiploma;
using ExaminationSystem.Features.Quizzes.Queries.GetPassRateByQuiz;
using MediatR;

namespace ExaminationSystem.Features.Analytics.Orchestrators
{
    public class AnalyticswithNoFiltersOrchestratorHandler : IRequestHandler<AnalyticswithNoFiltersOrchestrator, AnalysticsWithNoFilterDTO>
    {
        private readonly IMediator _mediator;

        public AnalyticswithNoFiltersOrchestratorHandler(IMediator mediator)
        {
          _mediator = mediator;
        }
        public async Task<AnalysticsWithNoFilterDTO> Handle(AnalyticswithNoFiltersOrchestrator request, CancellationToken cancellationToken)
        {
            var quizzes_per_rate=await _mediator.Send(new GetPassRateByQuizQuery(request.Filters), cancellationToken);

            var avg_score_by_diploma=await _mediator.Send(new GetAverageScorePerDiploamQuery(request.Filters), cancellationToken);

            var attempts_over_time=await _mediator.Send(new GetAttemptsOverTimeQuery(request.Filters), cancellationToken);

            // top_failed_questions
            var top_failed_questions = await _mediator.Send(new GetTopFailedQuestionQuery(request.Filters), cancellationToken);    

            return new AnalysticsWithNoFilterDTO()
            {
                pass_rate_by_quiz = quizzes_per_rate,
                Avg_score_by_diploma = avg_score_by_diploma,
                attempts_over_time = attempts_over_time,
                top_failed_questions = top_failed_questions
            };
           
        }
    }
}
