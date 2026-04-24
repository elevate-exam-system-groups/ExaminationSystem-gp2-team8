namespace ExaminationSystem.Features.AnswerQuestion
{
    public class GetAnswerDetailsDTO
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public string SelectedAnswer { get; set; } = string.Empty;

        public string CorrectAnswer { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
