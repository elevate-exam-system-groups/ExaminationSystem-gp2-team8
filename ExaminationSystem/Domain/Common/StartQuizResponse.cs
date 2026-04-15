namespace ExaminationSystem.Domain.Common
{
    public class QuizAttemptDto
    {
        public int AttemptId { get; set; }

        public int QuizId { get; set; }

        public string QuizTitle { get; set; } = null!;

        public int DurationMinutes { get; set; }

        public string? Instructions { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime Deadline { get; set; }

        public string Status { get; set; } = null!;
    }
}
