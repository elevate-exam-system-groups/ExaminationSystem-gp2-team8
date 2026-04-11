namespace ExaminationSystem.Features.Diplomas.DTOS
{
    public class DiplomapublishedDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int QuizCount { get; set; }
    }
}
