using ExaminationSystem.Domain.Entities;

namespace ExaminationSystem.Features.Diplomas.DTOS
{
    public class UpdateDiplomaDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

    }
}
