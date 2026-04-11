using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities
{
    public class Diploma
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int QuizCount { get; set; }
        public Status status { get; set; }

        public bool IsDeleted { get; set; }//Soft Delete

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User CreatedByUser { get; set; } = null!;
        public int CreatedByUserId { get; set; }//FK
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
