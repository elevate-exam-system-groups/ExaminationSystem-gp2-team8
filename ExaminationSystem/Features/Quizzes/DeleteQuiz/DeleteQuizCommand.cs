using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.DeleteQuiz
{
    public record DeleteQuizCommand(int quizId) : IRequest<ApiResponse<bool>>
    {
    }
}
