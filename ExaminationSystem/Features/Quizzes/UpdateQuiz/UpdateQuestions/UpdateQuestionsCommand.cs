using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.UpdateQuiz.UpdateQuestions
{
    public record UpdateQuestionsCommand(int questionId, CreateQuestionsforQuizDto dto) : IRequest<ApiResponse<bool>>
    {
    }
}
