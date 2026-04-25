using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.PublishQuiz
{
    public record PublishQuizCommand(int QuizId) : IRequest<PublishQuizDto>;
}
