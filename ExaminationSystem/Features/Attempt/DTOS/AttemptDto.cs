namespace ExaminationSystem.Features.Attempt.DTOS
{
    public class AttemptDto
    {
        public int Id { get; set; }
        public float Score { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
