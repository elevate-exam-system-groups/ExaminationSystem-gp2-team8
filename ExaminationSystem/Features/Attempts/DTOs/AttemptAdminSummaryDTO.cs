namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class AttemptAdminSummaryDTO
    {
        public int attemptedId { get; set; }
        public int studentId { get; set; }

        public string studentName { get; set; } = default!;
        public string quiztitle { get; set; } = default!;
        public float score { get; set; }
        public string status { get; set; } = default!;
        public DateTime submittedAt { get; set; }
        public ICollection<QuestionAttemptDTO> Questions { get; set; } = [];
    }
}
