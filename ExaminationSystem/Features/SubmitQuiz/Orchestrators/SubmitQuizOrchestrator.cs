using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.SubmitQuiz.Commands;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using ExaminationSystem.Features.SubmitQuiz.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.SubmitQuiz.Orchestrators
{
    public record SubmitQuizOrchestrator(int UserId, int QuizId, ICollection<QuizAnswerDto> QuizAnswers)
        : IRequest<Result<SubmitQuizResponseDto>>;

    public class SubmitQuizOrchestratorHandler : IRequestHandler<SubmitQuizOrchestrator, Result<SubmitQuizResponseDto>>
    {

        private readonly IMediator _mediator;


        public SubmitQuizOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Result<SubmitQuizResponseDto>> Handle(
    SubmitQuizOrchestrator request,
    CancellationToken cancellationToken)
        {
            await _mediator.Send(new GetUserValidationQuery(request.UserId), cancellationToken);

            await _mediator.Send(new QuizValidationQuery(request.QuizId), cancellationToken);

            await _mediator.Send(new CheckMaxAttemptsOrchestrator(request.UserId, request.QuizId), cancellationToken);

            await _mediator.Send(new ValidateQuizSubmissionOrchestrator(
                request.QuizId,
                request.UserId,
                request.QuizAnswers),
                cancellationToken);

            var evaluation = await _mediator.Send(
                new EvaluateQuizSubmissionOrchestrator(
                    request.QuizId,
                    request.UserId,
                    request.QuizAnswers),
                cancellationToken);

            if (!evaluation.IsSuccess)
                return Result<SubmitQuizResponseDto>.Fail(evaluation.Message);

            var result = await _mediator.Send(
                new CalculateQuizResultQuery(evaluation.Data),
                cancellationToken);

            var attemptResult = await _mediator.Send(
          new SubmitQuizAttemptCommand(
              request.UserId,
              request.QuizId,
              result.Data.Score,
              result.Data.IsPassed),
          cancellationToken);

            return Result<SubmitQuizResponseDto>.Success(new SubmitQuizResponseDto
            {
                AttemptId = attemptResult.Data.AttemptId,
                QuizId = request.QuizId,
                UserId = request.UserId,
                Score = result.Data.Score,
                IsPassed = result.Data.IsPassed,
                SubmittedAt = DateTime.UtcNow
            });
        }

    }
}
