namespace ExaminationSystem.Domain.Entities
{
    public class Admin: User
    {
        public ICollection<Diploma> CreatedDiplomas { get; set; } = new List<Diploma>();
        public ICollection<Quiz> CreatedQuizzes { get; set; } = new List<Quiz>();
        public ICollection<Question> CreatedQuestions { get; set; } = new List<Question>();
    }
}
