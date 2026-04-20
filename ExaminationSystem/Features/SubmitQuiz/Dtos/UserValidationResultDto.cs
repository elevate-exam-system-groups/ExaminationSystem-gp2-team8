namespace ExaminationSystem.Features.SubmitQuiz.Dtos
{
    public class UserValidationResultDto
    {
        public int UserId { get; set; }
        public bool IsFound { get; set; }
        public bool IsActive { get; set; }
    }
}
