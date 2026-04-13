
namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public record CreateQuizResponse(
        int QuizId,
        string Title,
        int DiplomaId,
        int DurationMinutes,
        int PassScore,
        int? MaxAttempts,
        string? Instructions,
        string Status);
}
