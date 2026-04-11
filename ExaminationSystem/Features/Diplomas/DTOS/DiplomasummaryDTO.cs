using ExaminationSystem.Features.Quizzes.DTOS;

namespace ExaminationSystem.Features.Diplomas.DTOS
{
    public class DiplomasummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public ICollection<QuizforDiplomaDTO> Quizzes { get; set; } = [];

    }
}
