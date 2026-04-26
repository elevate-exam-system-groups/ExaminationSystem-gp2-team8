using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record ValidateQuizQuestionsQuery(
        int QuizId,
        ICollection<QuizAnswerDto> Answers
    ) : IRequest<Result<ValidateSubmissionResultDto>>;

    public class ValidateQuizQuestionsQueryHandler
        : IRequestHandler<ValidateQuizQuestionsQuery, Result<ValidateSubmissionResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateQuizQuestionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ValidateSubmissionResultDto>> Handle(
            ValidateQuizQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var questions = await _unitOfWork.Repository<Question>()
                .GetAll(asNoTracking: true)
                .Where(q => q.QuizId == request.QuizId && !q.IsDeleted)
                .Select(q => q.Id)
                .ToListAsync(cancellationToken);

            if (!questions.Any())
                return Result<ValidateSubmissionResultDto>.Fail("No questions found for this quiz");

            var submittedQuestionIds = request.Answers
                .Select(a => a.QuestionId)
                .Distinct()
                .ToList();

            if (submittedQuestionIds.Count != questions.Count)
            {
                return Result<ValidateSubmissionResultDto>.Success(new ValidateSubmissionResultDto
                {
                    IsValid = false,
                    Message = "Missing or extra answers"
                });
            }

            var questionsSet = questions.ToHashSet();

            var hasInvalidQuestions = submittedQuestionIds
                .Any(id => !questionsSet.Contains(id));

            if (hasInvalidQuestions)
            {
                return Result<ValidateSubmissionResultDto>.Success(new ValidateSubmissionResultDto
                {
                    IsValid = false,
                    Message = "Invalid question detected"
                });
            }

            return Result<ValidateSubmissionResultDto>.Success(new ValidateSubmissionResultDto
            {
                IsValid = true,
                Message = "Valid submission"
            });
        }
    }
}