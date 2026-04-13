using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.DeleteQuiz.DeleteQuestion
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, ApiResponse<bool>>
    {
        private readonly ExamAppDbContext _dbContext;

        public DeleteQuestionCommandHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<bool>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _dbContext.Questions
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.Id == request.questionId, cancellationToken);

            if (question == null) return ApiResponse<bool>.FailureResponse("Question Not Found", "404");

            if (question.Quiz.Status == Status.published)
                return ApiResponse<bool>.FailureResponse("Cannot delete question while quiz is published", "409");

            question.IsDeleted = true;
            question.DeletedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
