namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class QuestionAttemptDTO
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;

        public int SelectedOptionId { get; set; }
        public string SelectedOptionText { get; set; } = null!;

        public string CorrectAnswer { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
