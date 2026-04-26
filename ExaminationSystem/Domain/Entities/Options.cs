namespace ExaminationSystem.Domain.Entities
{
    public class Options
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string OptionText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public bool IsDeleted { get; set; }
    }
}
