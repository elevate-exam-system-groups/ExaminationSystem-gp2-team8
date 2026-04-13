using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.UpdateQuiz.UpdateQuestions
{
    public class UpdateQuestionsCommandHandler : IRequestHandler<UpdateQuestionsCommand, ApiResponse<bool>>
    {
        private readonly ExamAppDbContext _dbContext;

        public UpdateQuestionsCommandHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<bool>> Handle(UpdateQuestionsCommand request, CancellationToken cancellationToken)
        {
            var question = await _dbContext.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == request.questionId, cancellationToken);

            if (question == null)
                return ApiResponse<bool>.FailureResponse("Question Not Found", "404");

            var incomingOptions = request.dto.Options;

            if (string.IsNullOrWhiteSpace(request.dto.QuestionText))
                return ApiResponse<bool>.FailureResponse("Question text is required", "422");


            question.QuestionText = request.dto.QuestionText;
            question.Explanation = request.dto.Explanation;

            if (incomingOptions.Count < 2)
                return ApiResponse<bool>.FailureResponse("At least 2 options required", "422");

            if (incomingOptions.Count(o => o.IsCorrect) != 1)
                return ApiResponse<bool>.FailureResponse("Exactly one correct option required", "422");

            if (incomingOptions.Any(o => string.IsNullOrWhiteSpace(o.OptionText)))
                return ApiResponse<bool>.FailureResponse("Option text is required", "422");

            _dbContext.Options.RemoveRange(question.Options.ToList());
            question.Options.Clear();

            foreach (var optDto in incomingOptions)
            {
                question.Options.Add(new Options
                {
                    Question = question,
                    OptionText = optDto.OptionText,
                    IsCorrect = optDto.IsCorrect
                });
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
