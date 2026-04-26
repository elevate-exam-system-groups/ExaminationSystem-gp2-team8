using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using ExaminationSystem.Features.SubmitQuiz.Queries;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Orchestrators
{
    public record EvaluateQuizSubmissionOrchestrator(
        int QuizId,
        int UserId,
        ICollection<QuizAnswerDto> Answers
    ) : IRequest<Result<EvaluateQuizResultDto>>;

    public class EvaluateQuizSubmissionOrchestratorHandler
        : IRequestHandler<EvaluateQuizSubmissionOrchestrator, Result<EvaluateQuizResultDto>>
    {
        private readonly IMediator _mediator;

        public EvaluateQuizSubmissionOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<EvaluateQuizResultDto>> Handle(
            EvaluateQuizSubmissionOrchestrator request,
            CancellationToken cancellationToken)
        {
           
            var questionsResult = await _mediator.Send( new GetQuizQuestionsQuery(request.QuizId),cancellationToken);

            if (!questionsResult.IsSuccess)
                return Result<EvaluateQuizResultDto>.Fail(questionsResult.Message);

            var questions = questionsResult.Data;

            
            var optionsMap = new Dictionary<int, List<OptionsDto>>();

            foreach (var q in questions)
            {
                var optionsResult = await _mediator.Send(
                    new GetQuestionOptionsQuery(q.QuestionId),
                    cancellationToken);

                if (!optionsResult.IsSuccess)
                    return Result<EvaluateQuizResultDto>.Fail(optionsResult.Message);

                optionsMap[q.QuestionId] = optionsResult.Data;
            }

            
            var evaluationResult = await _mediator.Send(new EvaluateAnswersQuery(questions, optionsMap, request.Answers),cancellationToken);

            if (!evaluationResult.IsSuccess)
                return Result<EvaluateQuizResultDto>.Fail(evaluationResult.Message);

            var evaluation = evaluationResult.Data;

            var passScoreResult = await _mediator.Send(new GetQuizPassScoreQuery(request.QuizId),cancellationToken);

            if (!passScoreResult.IsSuccess)
                return Result<EvaluateQuizResultDto>.Fail(passScoreResult.Message);

            var passScore = passScoreResult.Data.PassScore;

            evaluation.IsPassed = evaluation.Percentage >= passScore;

            return Result<EvaluateQuizResultDto>.Success(evaluation);
        }
    }
}