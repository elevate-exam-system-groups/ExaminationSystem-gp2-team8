namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public class QuizDetailsDTO
    {
        public int Id { get; set; }
        public string QuizTitle { get; set; } = default!;
        public int QuizDuarion { get; set; }
    }
}
