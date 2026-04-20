namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class SaveQuizAttemptResultDto
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }

        public double Score { get; set; }
        public bool IsPassed { get; set; }

        public int CorrectAnswersCount { get; set; }
        public int WrongAnswersCount { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
