using MediatR;

namespace ExaminationSystem.Features.Quizzes.UnpublishQuiz
{
    public record UnpublishQuizCommand(int id) : IRequest<bool>
    {
    }
}
