using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.StartQuiz.DTOS;
using MediatR;

namespace ExaminationSystem.Features.StartQuiz.Queries
{
    public record GetQuizMaxAttemptsQuery(int QuizId)
        : IRequest<Result<QuizMaxAttemptsDto>>;

    public class GetQuizMaxAttemptsQueryHandler
        : IRequestHandler<GetQuizMaxAttemptsQuery, Result<QuizMaxAttemptsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuizMaxAttemptsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<QuizMaxAttemptsDto>> Handle(
            GetQuizMaxAttemptsQuery request,
            CancellationToken cancellationToken)
        {
            var quiz = await _unitOfWork.Repository<Quiz>()
                .GetByIdAsync(request.QuizId);

            if (quiz == null)
                return Result<QuizMaxAttemptsDto>.Fail("Quiz not found");

            return Result<QuizMaxAttemptsDto>.Success(new QuizMaxAttemptsDto
            {
                MaxAttempts = quiz.MaxAttempts
            });
        }
    }
}
