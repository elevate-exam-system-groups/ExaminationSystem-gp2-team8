using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities
{
    public class Quiz : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Title { get; set; } = null!;
        public int DurationMinutes { get; set; }

        public int PassScore { get; set; }

        public int? MaxAttempts { get; set; } // optional

        public Status Status { get; set; }

        public string? Instructions { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }//Soft Delete
        //  FK
        public int CreatedByUserId { get; set; }
        public Admin CreatedByUser { get; set; } = null!;

        public int DiplomaId { get; set; }
        public Diploma Diploma { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Attempts> Attempts { get; set; } = new List<Attempts>();
    }
}
