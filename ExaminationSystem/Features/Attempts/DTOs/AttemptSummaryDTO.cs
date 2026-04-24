namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class AttemptSummaryDTO
    {
        public int attemptedId { get; set; }
        public int studentId { get; set; }

        public int QuizId { get; set; }
        public float score { get; set; }
        public string status { get; set; } = default!;
        public DateTime submittedAt { get; set; }
        
    }
}
