using ExaminationSystem.Domain.Entities;

namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class OptionsDto
    {
        public int OptionId { get; set; }   
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
