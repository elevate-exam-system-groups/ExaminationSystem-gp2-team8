using MediatR;

namespace ExaminationSystem.Features.Quizzes.DeleteQuiz
{
    public record DeleteQuizCommand(int quizId) : IRequest<bool>
    {
    }
}
