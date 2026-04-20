using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.StartQuiz.Orchestrators;
using ExaminationSystem.Features.StartQuiz.Queries;
using ExaminationSystem.Features.StartQuiz.DTOS;

using MediatR;
using QuizAttemptDto = ExaminationSystem.Features.StartQuiz.DTOS.QuizAttemptDto;

public record StartQuizOrchestrator(int UserId, int QuizId)
    : IRequest<Result<QuizAttemptDto>>;

public class StartQuizOrchestratorHandler
    : IRequestHandler<StartQuizOrchestrator, Result<QuizAttemptDto>>
{
    private readonly IMediator _mediator;

    public StartQuizOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<QuizAttemptDto>> Handle(
        StartQuizOrchestrator request,
        CancellationToken cancellationToken)
    {
        // 1) Check Quiz Availability
        var isAvailable = await _mediator.Send(
            new IsQuizAvailableQuery(request.QuizId),
            cancellationToken);

        if (!isAvailable)
            return Result<QuizAttemptDto>.Fail("Quiz not available");

        // 2) Get Quiz
        var quizResult = await _mediator.Send(
            new GetQuizByIdQuery(request.QuizId),
            cancellationToken);

        if (!quizResult.IsSuccess || quizResult.Data is null)
            return Result<QuizAttemptDto>.Fail("Quiz not found");

        var quiz = quizResult.Data;

        // 3) Get Active User
        var user = await _mediator.Send(
            new GetActiveUserQuery(request.UserId),
            cancellationToken);

        if (user is null)
            return Result<QuizAttemptDto>.Fail("User not found or inactive");

        // 4) Check Max Attempts
        var attemptsCheck = await _mediator.Send(
            new CheckMaxAttemptsOrchestrator(request.UserId, request.QuizId),
            cancellationToken);

        if (!attemptsCheck.IsSuccess || !attemptsCheck.Data.IsAllowed)
            return Result<QuizAttemptDto>.Fail(
                attemptsCheck.Data?.Message ?? "Max attempts reached");

        // 5) Check Active Attempt
        var activeAttemptResult = await _mediator.Send(
            new CheckActiveAttemptQuery(request.UserId, request.QuizId),
            cancellationToken);

        if (!activeAttemptResult.IsSuccess)
            return Result<QuizAttemptDto>.Fail("Active attempt check failed");

        if (activeAttemptResult.Data)
            return Result<QuizAttemptDto>.Fail("You already have an active attempt");

        // 6) Create Attempt
        var attemptResult = await _mediator.Send(
            new CreateAttemptCommand(request.UserId, request.QuizId, quiz.DurationMinutes),
            cancellationToken);

        if (!attemptResult.IsSuccess || attemptResult.Data is null)
            return Result<QuizAttemptDto>.Fail("Failed to create attempt");

        var attempt = attemptResult.Data;

        // 7) Final DTO
        return Result<QuizAttemptDto>.Success(new QuizAttemptDto
        {
            AttemptId = attempt.AttemptId,
            QuizId = quiz.Id,
            QuizTitle = quiz.Title,
            DurationMinutes = quiz.DurationMinutes,
            Instructions = quiz.Instructions,
            StartTime = attempt.StartTime,
            Deadline = attempt.Deadline,
            Status = attempt.Status
        });
    }

}