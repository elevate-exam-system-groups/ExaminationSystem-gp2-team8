using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Quizzes.DTOS;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.UpdateQuiz
{
    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, ApiResponse<CreateQuizResponse>>
    {
        private readonly ExamAppDbContext _dbContext;

        public UpdateQuizCommandHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<CreateQuizResponse>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _dbContext.Quizzes.FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);
            if (quiz == null) return ApiResponse<CreateQuizResponse>.FailureResponse("Quiz Not Found", "404");

            var diplomaExists = await _dbContext.Diplomas.AnyAsync(d => d.Id == request.dto.DiplomaId, cancellationToken);
            if (!diplomaExists) return ApiResponse<CreateQuizResponse>.FailureResponse("Diploma Not Found", "404");

            if (string.IsNullOrWhiteSpace(request.dto.Title))
                return ApiResponse<CreateQuizResponse>.FailureResponse("Title is required", "422");

            if (request.dto.DurationMinutes <= 0)
                return ApiResponse<CreateQuizResponse>.FailureResponse("DurationMinutes must be a positive integer", "422");

            if (request.dto.PassScore is < 0 or > 100)
                return ApiResponse<CreateQuizResponse>.FailureResponse("PassScore must be between 0 and 100", "422");

            quiz.Title = request.dto.Title;
            quiz.DiplomaId = request.dto.DiplomaId;
            quiz.DurationMinutes = request.dto.DurationMinutes;
            quiz.Instructions = request.dto.Instructions;
            quiz.MaxAttempts = request.dto.MaxAttempts;
            quiz.PassScore = request.dto.PassScore;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<CreateQuizResponse>.SuccessResponse(new CreateQuizResponse(
                quiz.Id,
                quiz.Title,
                quiz.DiplomaId,
                quiz.DurationMinutes,
                quiz.PassScore,
                quiz.MaxAttempts,
                quiz.Instructions,
                quiz.Status.ToString()
                ));
        }
    }
}
