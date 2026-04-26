using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.SubmitQuiz.Commands;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Orchestrators
{
    public record SaveQuizAttemptOrchestrator(
        int UserId,
        int QuizId,
        double Score,
        bool IsPassed,
        int AttemptId,
        ICollection<QuizAnswerDto> Answers
    )
        : IRequest<Result<SaveQuizAttemptResultDto>>;

    public class SaveQuizAttemptOrchestratorHandler
        : IRequestHandler<SaveQuizAttemptOrchestrator, Result<SaveQuizAttemptResultDto>>
    {
        private readonly IMediator _mediator;

        public SaveQuizAttemptOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<SaveQuizAttemptResultDto>> Handle(
            SaveQuizAttemptOrchestrator request,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new SubmitQuizAttemptCommand(
                request.UserId,
                request.QuizId,
                request.Score,
                request.IsPassed
            ));

            await _mediator.Send(new SaveStudentAnswersCommand(
                request.UserId,
                request.AttemptId,
                request.Answers
            ));

            return Result<SaveQuizAttemptResultDto>.Success(new SaveQuizAttemptResultDto
            {
                AttemptId = request.AttemptId,
                QuizId = request.QuizId,
                UserId = request.UserId,
                Score = request.Score,
                IsPassed = request.IsPassed,
                CorrectAnswersCount = 0,
                WrongAnswersCount = 0,
                SubmittedAt = DateTime.UtcNow
            });
        }
    }
}