using ExaminationSystem.Domain.Entities;

namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public class CreateQuestionsforQuizDto
    {
        public string QuestionText { get; set; } = null!;
        public string? Explanation { get; set; }
        public ICollection<CreateOptionsforQuestionDto> Options { get; set; } = new List<CreateOptionsforQuestionDto>();
    }
}
