using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record QuizValidationQuery(int QuizId) : IRequest<Result<QuizValidationResultDto>>;

    public class QuizValidationQueryHandler : IRequestHandler<QuizValidationQuery, Result<QuizValidationResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        public QuizValidationQueryHandler(IUnitOfWork unitOfWork)

        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<QuizValidationResultDto>> Handle(
    QuizValidationQuery request,
    CancellationToken cancellationToken)
        {
            var quiz = await _unitOfWork.Repository<Quiz>()
                .GetByIdAsync(request.QuizId);

            if (quiz == null)
            {
                return Result<QuizValidationResultDto>.Fail("Quiz not found");
            }
            if (quiz.IsDeleted)
            {
                return Result<QuizValidationResultDto>.Fail("Quiz is deleted");
            }

            if (quiz.Status != Status.Published)
            {
                return Result<QuizValidationResultDto>.Fail("Quiz is not published");
            }
            var result = new QuizValidationResultDto
            {
                QuizId = quiz.Id,
                IsFound = true,
                IsDeleted = quiz.IsDeleted,
                IsPublished = quiz.Status == Status.Published
            };

            return Result<QuizValidationResultDto>.Success(result);
        }
    }
}
