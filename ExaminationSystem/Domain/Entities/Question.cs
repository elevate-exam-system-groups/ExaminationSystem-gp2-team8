namespace ExaminationSystem.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public string QuestionText { get; set; } = null!;

        public string? Explanation { get; set; }

        public int OrderIndex { get; set; }

        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }//Soft Delete
        //public bool IsCorrect { get; set; }

        public int CreatedByUserId { get; set; }

        public User CreatedByUser { get; set; } = null!;

        public ICollection<Options> Options { get; set; } = new List<Options>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
