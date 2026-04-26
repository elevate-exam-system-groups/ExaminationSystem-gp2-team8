namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public class PassRateByQuizDTO
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }= string.Empty;
        public int TotalAttempts { get; set; }
        public int PassedAttempts { get; set; }
        public double PassRate { get; set; }
    }
}
