using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz.CreateQuestions
{
    public class CreateQuestionsCommandHandler : IRequestHandler<CreateQuestionsCommand, ApiResponse<int>>
    {
        private readonly ExamAppDbContext _dbContext;

        public CreateQuestionsCommandHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<int>> Handle(CreateQuestionsCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _dbContext.Quizzes
                .FirstOrDefaultAsync(q => q.Id == request.quizId, cancellationToken);

            if (quiz is null) return ApiResponse<int>.FailureResponse("Quiz Not Found", "404");

            var dto = request.dto;

            if (string.IsNullOrWhiteSpace(dto.QuestionText))
                return ApiResponse<int>.FailureResponse("Question text is required", "422");

            if (dto.Options.Count < 2)
                return ApiResponse<int>.FailureResponse("Question must have at least 2 options", "422");

            if (dto.Options.Count(o => o.IsCorrect) != 1)
                return ApiResponse<int>.FailureResponse("Exactly one correct option required", "422");

            if (dto.Options.Any(o => string.IsNullOrWhiteSpace(o.OptionText)))
                return ApiResponse<int>.FailureResponse("Option text is required", "422");

            var question = new Question
            {
                QuestionText = dto.QuestionText,
                Explanation = dto.Explanation,
                Quiz = quiz,
            };

            foreach (var o in dto.Options)
            {
                question.Options.Add(new Options
                {
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect,
                    Question = question,
                });
            }

            quiz.Questions.Add(question);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return ApiResponse<int>.SuccessResponse(question.Id);
        }
    }
}
