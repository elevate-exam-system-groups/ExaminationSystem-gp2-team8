namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class SubmitQuizResultDto
    {
        public int AttemptId { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
    }
}
