namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class SubmitQuizResponseDto
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }

        public double Score { get; set; }
        public bool IsPassed { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
