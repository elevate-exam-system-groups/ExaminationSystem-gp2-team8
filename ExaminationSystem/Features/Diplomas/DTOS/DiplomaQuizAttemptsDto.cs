using ExaminationSystem.Features.Quizzes.DTOS;

namespace ExaminationSystem.Features.Diplomas.DTOS
{
    public class DiplomaQuizAttemptsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int QuizCount { get; set; }
        public List<QuizAttemptsDto> Quizzes { get; set; } = [];
    }
}
