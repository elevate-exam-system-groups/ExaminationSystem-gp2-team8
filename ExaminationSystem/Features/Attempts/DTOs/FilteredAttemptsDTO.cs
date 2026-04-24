namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class FilteredAttemptsDTO
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int studentId { get; set; }
        public float score { get; set; }
        public string status { get; set; } = default!;
        public DateTime SubmittedAt { get; set; }
    }
}
