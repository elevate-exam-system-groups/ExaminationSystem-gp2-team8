using ExaminationSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Domain.Entities
{
    public class User:IdentityUser<int>, IBaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string FullName { get; set; } = null!;

        public string? OtpHash { get; set; }

        public DateTime? OtpExpiry { get; set; }

        public string? RefreshToken { get; set; }

        public UserStatus Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Diploma> CreatedDiplomas { get; set; } = new List<Diploma>();
        public ICollection<Quiz> CreatedQuizzes { get; set; } = new List<Quiz>();
        public ICollection<Question> CreatedQuestions { get; set; } = new List<Question>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Attempts> Attempts { get; set; } = new List<Attempts>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

        // Authentication //

    }
}
