namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class GetAttemptsOverTimeDTO
    {
        public DateTime SubmittedAt { get; set; }
        public int AttemptCount { get; set; }
    }
}
