namespace ExaminationSystem.Features.Attempts.DTOs
{
    public record AttemptResultDetailDto(
        int AttemptId,
        string QuizTitle,
        float Score,
        bool Passed,
        string Status,
        DateTime SubmittedAt,
        int TotalQuestions,
        int CorrectCount,
        IReadOnlyList<AttemptQuestionDetailDto> PerQuestion
    );
}
