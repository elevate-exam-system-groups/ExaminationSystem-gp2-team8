using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.AnswerQuestion.DTOs
{
    public class SubmitAnswerRequestDto
    {
        [Required(ErrorMessage = "question_id is required.")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "selected_option_id is required.")]
        public int SelectedOptionId { get; set; }
    }
}
