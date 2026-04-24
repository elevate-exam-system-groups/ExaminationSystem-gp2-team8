namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class QuestionAttemptsDTO
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public string SelectedAnswer { get; set; } = string.Empty;

        public string CorrectAnswer { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
