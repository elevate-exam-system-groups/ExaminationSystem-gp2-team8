using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities
{
    public class Attempts : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public float score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public AttemptStatus Attempt { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
