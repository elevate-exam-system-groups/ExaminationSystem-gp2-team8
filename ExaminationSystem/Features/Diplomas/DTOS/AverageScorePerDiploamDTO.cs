namespace ExaminationSystem.Features.Diplomas.DTOS
{
    public class AverageScorePerDiploamDTO
    {
        public int DiplomaId { get; set; }
        public string DiplomaTitle { get; set; } = default!;
        public float AverageScore { get; set; }
    }
}
