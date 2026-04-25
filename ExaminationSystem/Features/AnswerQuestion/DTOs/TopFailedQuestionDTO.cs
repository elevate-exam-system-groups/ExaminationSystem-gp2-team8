namespace ExaminationSystem.Features.AnswerQuestion.DTOs
{
    public class TopFailedQuestionDTO
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int FailedCount { get; set; }
        public int TotalCount { get; set; }
        public double CorrectRate { get; set; }
    }
}
