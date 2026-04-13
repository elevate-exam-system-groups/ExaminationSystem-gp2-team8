using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.DeleteQuiz
{
    public class DeleteQuizCommandHandler : IRequestHandler<DeleteQuizCommand, ApiResponse<bool>>
    {
        private readonly ExamAppDbContext _dbContext;

        public DeleteQuizCommandHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<bool>> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _dbContext.Quizzes.FindAsync(request.quizId);
            if (quiz is null) return ApiResponse<bool>.FailureResponse("Quiz Not Found", "404");

            //if (quiz.Status == Domain.Enums.Status.published) return ApiResponse<bool>.FailureResponse("Can not delete a published quiz");

            quiz.IsDeleted = true;
            quiz.DeletedAt = DateTime.UtcNow;

            _dbContext.Quizzes.Update(quiz);
            await _dbContext.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);

        }
    }
}
