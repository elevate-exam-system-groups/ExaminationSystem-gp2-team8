using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.StartQuiz.DTOS;
using ExaminationSystem.Features.StartQuiz.Queries;
using MediatR;

namespace ExaminationSystem.Features.StartQuiz.Orchestrators
{
    public record CheckMaxAttemptsQueryorchestrator(int UserId, int QuizId)
        : IRequest<Result<CheckMaxAttemptsResultDto>>;

    public class CheckMaxAttemptsQueryorchestratorHandler
        : IRequestHandler<CheckMaxAttemptsQueryorchestrator, Result<CheckMaxAttemptsResultDto>>
    {
        private readonly IMediator _mediator;

        public CheckMaxAttemptsQueryorchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<CheckMaxAttemptsResultDto>> Handle(
            CheckMaxAttemptsQueryorchestrator request,
            CancellationToken cancellationToken)
        {
            // 1) Get MaxAttempts
            var quizResult = await _mediator.Send(
                new GetQuizMaxAttemptsQuery(request.QuizId),
                cancellationToken);

            if (!quizResult.IsSuccess || quizResult.Data is null)
                return Result<CheckMaxAttemptsResultDto>.Fail(
                    quizResult.Message ?? "Quiz validation failed");

            // 2) Get Attempts Count
            var attemptsResult = await _mediator.Send(
                new GetUserAttemptsCountQuery(request.UserId, request.QuizId),
                cancellationToken);

            if (!attemptsResult.IsSuccess || attemptsResult.Data is null)
                return Result<CheckMaxAttemptsResultDto>.Fail(
                    attemptsResult.Message ?? "Attempts validation failed");

            var maxAttempts = quizResult.Data.MaxAttempts;
            var attemptsCount = attemptsResult.Data.AttemptsCount;

            // 3) Business Logic
            if (maxAttempts.HasValue && attemptsCount >= maxAttempts.Value)
            {
                return Result<CheckMaxAttemptsResultDto>.Success(new CheckMaxAttemptsResultDto
                {
                    IsAllowed = false,
                    AttemptsCount = attemptsCount,
                    Message = "Maximum attempts reached"
                });
            }

            return Result<CheckMaxAttemptsResultDto>.Success(new CheckMaxAttemptsResultDto
            {
                IsAllowed = true,
                AttemptsCount = attemptsCount,
                Message = "Allowed"
            });
        }
    }
}