namespace ExaminationSystem.Features.Students.DTOs
{
    public record StudentStatsDto (int totalQuizzes, double avgScore, double passRate);
}