namespace ExaminationSystem.Domain.Entities
{
    public class Options : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string OptionText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
