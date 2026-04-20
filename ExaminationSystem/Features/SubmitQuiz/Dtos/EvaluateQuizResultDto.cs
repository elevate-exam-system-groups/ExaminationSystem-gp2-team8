namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class EvaluateQuizResultDto
    {
        public int Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public double Percentage { get; set; }
        public bool IsPassed { get; set; }
    }
}
