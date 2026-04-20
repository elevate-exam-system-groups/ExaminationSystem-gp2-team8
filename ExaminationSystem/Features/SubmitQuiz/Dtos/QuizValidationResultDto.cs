namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class QuizValidationResultDto
    {
        public int QuizId { get; set; }
        public bool IsFound { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
    }
}
