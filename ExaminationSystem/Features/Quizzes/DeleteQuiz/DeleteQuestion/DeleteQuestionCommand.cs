using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.DeleteQuiz.DeleteQuestion
{
    public record DeleteQuestionCommand(int questionId) : IRequest<ApiResponse<bool>>
    {
    }
}
