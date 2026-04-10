namespace ExaminationSystem.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }

        public int QuizId { get; set; }

        public string QuestionText { get; set; } = null!;

        public string? Explanation { get; set; }

        public int OrderIndex { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedByUserId { get; set; }

        public User CreatedByUser { get; set; } = null!;

        public IEnumerable<Options> Options { get; set; } = [];
    }
}
