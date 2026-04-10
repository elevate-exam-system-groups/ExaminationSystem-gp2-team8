using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features.Attempts.DTOs
{
    public record QuizHistoryDto(
        int Id,
        string QuizTitle,
        double score,
        string Status,
        bool Passed,
        DateTime? SubmittedAt
        );
}
