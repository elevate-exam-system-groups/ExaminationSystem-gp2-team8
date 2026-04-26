using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class QuizDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public int DurationMinutes { get; set; }

        public int PassScore { get; set; }

        public int? MaxAttempts { get; set; }

        public Status Status { get; set; }

        public string? Instructions { get; set; }
    }
}
