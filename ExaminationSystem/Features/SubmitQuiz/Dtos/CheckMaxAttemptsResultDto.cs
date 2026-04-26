namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class CheckMaxAttemptsResultDto
    {
        public bool IsAllowed { get; set; }
        public string Message { get; set; } = string.Empty;
        public int AttemptsCount { get; set; }
    }
}
