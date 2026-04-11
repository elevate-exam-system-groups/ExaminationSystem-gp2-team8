using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public class QuizforDiplomaDTO
    {

        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public Status Status { get; set; }

        public int attemptCount { get; set; }

        public double LastScore { get; set; }

    }
}
