namespace ExaminationSystem.Features.StartQuiz.DTOS
{
    public class CheckMaxAttemptsResultDto
    {
        public bool IsAllowed { get; set; }
        public int AttemptsCount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
