namespace ExaminationSystem.Features.Attempts.DTOs
{
    public record AttemptQuestionDetailDto(
        int QuestionId,
        string QuestionText,
        string? StudentAnswer,
        string CorrectAnswer,
        bool IsCorrect,
        string? Explanation
    );
}
