namespace ExaminationSystem.Domain.Entities
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Student User { get; set; } = null!;
        public int AttemptId { get; set; }
        public Attempts Attempt { get; set; } = null!;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int SelectedOptionId { get; set; }
        public Options SelectedOption { get; set; } = null!;
    }
}
