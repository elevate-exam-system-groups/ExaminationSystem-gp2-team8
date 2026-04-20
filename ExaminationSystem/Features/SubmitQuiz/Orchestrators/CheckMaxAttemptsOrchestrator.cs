using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record CheckMaxAttemptsOrchestrator(int UserId, int QuizId)
        : IRequest<Result<CheckMaxAttemptsResultDto>>;

    public class CheckMaxAttemptsOrchestratorHandler
        : IRequestHandler<CheckMaxAttemptsOrchestrator, Result<CheckMaxAttemptsResultDto>>
    {
        private readonly IMediator _mediator;

        public CheckMaxAttemptsOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<CheckMaxAttemptsResultDto>> Handle(
            CheckMaxAttemptsOrchestrator request,
            CancellationToken cancellationToken)
        {
            var quizResult = await _mediator.Send(
                new GetQuizMaxAttemptsQuery(request.QuizId),
                cancellationToken);

            if (!quizResult.IsSuccess || quizResult.Data is null)
                return Result<CheckMaxAttemptsResultDto>.Fail(
                    quizResult.Message ?? "Quiz validation failed");

            var maxAttempts = quizResult.Data.MaxAttempts;

            var attemptsResult = await _mediator.Send(
                new GetUserAttemptsCountQuery(request.UserId, request.QuizId),
                cancellationToken);

            if (!attemptsResult.IsSuccess || attemptsResult.Data is null)
                return Result<CheckMaxAttemptsResultDto>.Fail(
                    attemptsResult.Message ?? "Attempts validation failed");

            var attemptsCount = attemptsResult.Data.AttemptsCount;

            // No limit rule
            if (!maxAttempts.HasValue)
            {
                return Result<CheckMaxAttemptsResultDto>.Success(new CheckMaxAttemptsResultDto
                {
                    IsAllowed = true,
                    AttemptsCount = attemptsCount,
                    Message = "Allowed"
                });
            }

            var isAllowed = attemptsCount < maxAttempts.Value;

            return Result<CheckMaxAttemptsResultDto>.Success(new CheckMaxAttemptsResultDto
            {
                IsAllowed = isAllowed,
                AttemptsCount = attemptsCount,
                Message = isAllowed ? "Allowed" : "Maximum attempts reached"
            });
        }
    }
}