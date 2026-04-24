namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class ViewResultDTO
    {
        public float score { get; set; }
        public string status { get; set; } = null!;
        public int TotalQuestions { get; set; }
        public int CorrectCount { get; set; }
        //per Question
        public ICollection<QuestionAttemptsDTO> Questions { get; set; } = [];
    }
}
