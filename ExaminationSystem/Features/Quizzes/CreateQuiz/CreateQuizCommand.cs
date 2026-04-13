using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz
{
    public record CreateQuizCommand(CreateQuizDto dto) :IRequest<ApiResponse<CreateQuizResponse>>
    {
    }
}
