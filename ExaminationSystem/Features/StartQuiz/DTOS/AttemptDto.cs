namespace ExaminationSystem.Features.StartQuiz.DTOS
{
    public class AttemptDto
    {
        public int AttemptId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
