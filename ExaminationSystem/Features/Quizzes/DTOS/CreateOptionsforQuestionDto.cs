using ExaminationSystem.Domain.Entities;

namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public class CreateOptionsforQuestionDto
    {
        public string OptionText { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
