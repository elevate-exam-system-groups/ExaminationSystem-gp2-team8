using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities
{
    public class Attempts : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public int UserId { get; set; }
        public Student User { get; set; } = null!;
        public float score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime Deadline { get; set; }

        public AttemptStatus Attempt { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

        //public int Id { get; set; }
        //public int UserId { get; set; }
        //public int QuizId { get; set; }
        //public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;
        public int? ScorePct { get; set; }
        public bool? Passed { get; set; }
        public int? CorrectCount { get; set; }
        public int? TotalQuestions { get; set; }
        //public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        //public DateTime Deadline { get; set; }  // server-side truth — StartedAt + DurationMinutes
        //public DateTime? SubmittedAt { get; set; }

        // Navigation
        //public User User { get; set; } = null!;
        //public Quiz Quiz { get; set; } = null!;
        //public ICollection<StudentAnswer> StudentAnswers { get; set; } = [];

        // Computed helpers (never stored)
        public bool IsExpired => DateTime.UtcNow >= Deadline;
        //public bool IsInProgress => Attempt == AttemptStatus.InProgress && !IsExpired;
    }
}
