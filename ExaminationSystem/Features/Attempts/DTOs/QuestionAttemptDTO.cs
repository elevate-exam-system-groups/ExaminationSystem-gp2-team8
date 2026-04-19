namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class QuestionAttemptDTO
    {
        public int QuestionId { get; set; }
        public string studentAnswer { get; set; } = null!;
        public string CorrectAnswer { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
