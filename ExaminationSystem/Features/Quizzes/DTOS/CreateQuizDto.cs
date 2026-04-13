using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public class CreateQuizDto
    {
        public string Title { get; set; } = null!;
        public int DiplomaId { get; set; }
        public int DurationMinutes { get; set; }
        public int PassScore { get; set; } = 60;
        public int? MaxAttempts { get; set; }
        public string? Instructions { get; set; }
    }
}
