using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.UpdateQuiz
{
    public record UpdateQuizCommand(int QuizId, UpdateQuizDto dto) :IRequest<CreateQuizResponse>
    {
    }
}
