namespace ExaminationSystem.Domain.Entities
{
    public class StudentAnswer : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int AttemptId { get; set; }
        public Attempts Attempt { get; set; } = null!;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int SelectedOptionId { get; set; }
        public Options SelectedOption { get; set; } = null!;
        // for submit answer
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    }
}
