using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Attempts.GetAttempts
{
    public record GetStudentAttemptQuery(int StudentId, int? quizId, int? diplomaId, int page, int perPage) :IRequest<PaginatedResult<QuizHistoryDto>>
    {
    }
}
