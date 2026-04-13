using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz.CreateQuestions
{
    public record CreateQuestionsCommand(int quizId, CreateQuestionsforQuizDto dto) : IRequest<ApiResponse<int>>
    {
    }
}
