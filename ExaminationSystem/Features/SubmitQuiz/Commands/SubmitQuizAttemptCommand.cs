using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Commands
{
    public record SubmitQuizAttemptCommand(
        int UserId,
        int QuizId,
        double Score,
        bool IsPassed
    ) : IRequest<Result<SubmitQuizAttemptResultDto>>;

    public class SubmitQuizAttemptCommandHandler
        : IRequestHandler<SubmitQuizAttemptCommand, Result<SubmitQuizAttemptResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubmitQuizAttemptCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SubmitQuizAttemptResultDto>> Handle(
            SubmitQuizAttemptCommand request,
            CancellationToken cancellationToken)
        {
            var attempt = new Attempts
            {
                UserId = request.UserId,
                QuizId = request.QuizId,
                score = (float)request.Score,
                StartTime = DateTime.UtcNow,
                Deadline = DateTime.UtcNow,
                Attempt = AttemptStatus.Submitted
            };

            await _unitOfWork.Repository<Attempts>().AddAsync(attempt);
            await _unitOfWork.SaveChangesAsync();

            return Result<SubmitQuizAttemptResultDto>.Success(
                new SubmitQuizAttemptResultDto
                {
                    AttemptId = attempt.Id
                });
        }
    }
}