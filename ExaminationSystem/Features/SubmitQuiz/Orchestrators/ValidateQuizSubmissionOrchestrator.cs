using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using ExaminationSystem.Features.SubmitQuiz.Queries;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Orchestrators
{
    public record ValidateQuizSubmissionOrchestrator(
        int QuizId,
        int UserId,
        ICollection<QuizAnswerDto> Answers
    ) : IRequest<Result<ValidateSubmissionResultDto>>;

    public class ValidateQuizSubmissionOrchestratorHandler
        : IRequestHandler<ValidateQuizSubmissionOrchestrator, Result<ValidateSubmissionResultDto>>
    {
        private readonly IMediator _mediator;

        public ValidateQuizSubmissionOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<ValidateSubmissionResultDto>> Handle(
            ValidateQuizSubmissionOrchestrator request,
            CancellationToken cancellationToken)
        {
            // STEP 1: User Validation
            var userResult = await _mediator.Send(
                new GetUserValidationQuery(request.UserId),
                cancellationToken);

            if (!userResult.IsSuccess)
                return Result<ValidateSubmissionResultDto>.Fail(userResult.Message);

            // STEP 2: Quiz Max Attempts
            var quizResult = await _mediator.Send(
                new GetQuizMaxAttemptsQuery(request.QuizId),
                cancellationToken);

            if (!quizResult.IsSuccess)
                return Result<ValidateSubmissionResultDto>.Fail(quizResult.Message);

            var maxAttemptsDto = quizResult.Data;

            // STEP 3: User Attempts Count
            var attemptsResult = await _mediator.Send(
                new GetUserAttemptsCountQuery(request.UserId, request.QuizId),
                cancellationToken);

            if (!attemptsResult.IsSuccess)
                return Result<ValidateSubmissionResultDto>.Fail(attemptsResult.Message);

            var attemptsDto = attemptsResult.Data;

            // STEP 4: Questions Validation
            var questionsValidationResult = await _mediator.Send(
                new ValidateQuizQuestionsQuery(request.QuizId, request.Answers),
                cancellationToken);

            if (!questionsValidationResult.IsSuccess)
                return Result<ValidateSubmissionResultDto>.Fail(questionsValidationResult.Message);

            var questionsValidation = questionsValidationResult.Data;

            if (!questionsValidation.IsValid)
            {
                return Result<ValidateSubmissionResultDto>.Success(new ValidateSubmissionResultDto
                {
                    IsValid = false,
                    Message = questionsValidation.Message
                });
            }

            // Business rule: no max attempts limit
            if (!maxAttemptsDto.MaxAttempts.HasValue)
            {
                return Result<ValidateSubmissionResultDto>.Success(new ValidateSubmissionResultDto
                {
                    IsValid = true,
                    Message = "Allowed"
                });
            }

            // Compare correctly (DTO property)
            if (attemptsDto.AttemptsCount >= maxAttemptsDto.MaxAttempts.Value)
            {
                return Result<ValidateSubmissionResultDto>.Success(new ValidateSubmissionResultDto
                {
                    IsValid = false,
                    Message = "Maximum attempts reached"
                });
            }

            return Result<ValidateSubmissionResultDto>.Success(new ValidateSubmissionResultDto
            {
                IsValid = true,
                Message = "All validations passed successfully"
            });
        }
    }
}