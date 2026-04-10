namespace ExaminationSystem.Features.Diplomas.DTOS
{
    public class DiplomaDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; }= null!;
        public int QuizCount { get; set; }
        public double StudentProgress { get; set; }
    }
}
