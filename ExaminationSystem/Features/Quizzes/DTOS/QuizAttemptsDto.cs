using ExaminationSystem.Features.Attempt.DTOS;

namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public class QuizAttemptsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int TotalAttempts { get; set; }
        public List<AttemptDto> Attempts { get; set; } = [];
    }
}
