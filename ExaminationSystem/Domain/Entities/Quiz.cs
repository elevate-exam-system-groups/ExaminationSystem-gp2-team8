using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities
{
    public class Quiz
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public int DurationMinutes { get; set; }

        public int PassScore { get; set; }

        public int? MaxAttempts { get; set; } // optional

        public Status Status { get; set; }

        public string? Instructions { get; set; }

        // Soft delete
        public bool IsDeleted { get; set; }

        //  FK
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public int DiplomaId { get; set; }
        public Diploma Diploma { get; set; } = null!;

        public ICollection<Attempts> Attempts { get; set; } = new List<Attempts>();


    }
}
