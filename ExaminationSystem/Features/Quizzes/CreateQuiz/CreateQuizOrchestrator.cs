using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Quizzes.DTOS;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz
{
    public class CreateQuizOrchestrator : IRequestHandler<CreateQuizCommand, ApiResponse<CreateQuizResponse>>
    {
        private readonly ExamAppDbContext _dbContext;

        public CreateQuizOrchestrator(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<CreateQuizResponse>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;

            var diploma = await _dbContext.Diplomas.AnyAsync(d => d.Id == dto.DiplomaId, cancellationToken);

            if (!diploma) return ApiResponse<CreateQuizResponse>.FailureResponse("Diploma Not Found", "404");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<CreateQuizResponse>.FailureResponse("Title is required", "422");

            if (dto.DurationMinutes <= 0)
                return ApiResponse<CreateQuizResponse>.FailureResponse("DurationMinutes must be a positive integer", "422");

            if (dto.PassScore is < 0 or > 100)
                return ApiResponse<CreateQuizResponse>.FailureResponse("PassScore must be between 0 and 100", "422");

            var quiz = new Quiz
            {
                Title = dto.Title,
                DiplomaId = dto.DiplomaId,
                DurationMinutes = dto.DurationMinutes,
                Instructions = dto.Instructions, 
                MaxAttempts = dto.MaxAttempts, 
                PassScore = dto.PassScore,
                Status = Domain.Enums.Status.Draft,
            };

            _dbContext.Quizzes.Add(quiz);
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
                )
            );
        }
    }
}
