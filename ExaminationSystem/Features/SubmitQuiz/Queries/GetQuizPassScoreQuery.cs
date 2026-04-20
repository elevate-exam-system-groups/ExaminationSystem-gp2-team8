using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record GetQuizPassScoreQuery(int QuizId)
        : IRequest<Result<QuizPassScoreDto>>;

    public class GetQuizPassScoreQueryHandler
        : IRequestHandler<GetQuizPassScoreQuery, Result<QuizPassScoreDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuizPassScoreQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<QuizPassScoreDto>> Handle(
            GetQuizPassScoreQuery request,
            CancellationToken cancellationToken)
        {
            var quiz = await _unitOfWork.Repository<Quiz>()
                .GetByIdAsync(request.QuizId);

            if (quiz == null)
                return Result<QuizPassScoreDto>.Fail("Quiz not found");

            return Result<QuizPassScoreDto>.Success(new QuizPassScoreDto
            {
                PassScore = quiz.PassScore
            });
        }
    }
}