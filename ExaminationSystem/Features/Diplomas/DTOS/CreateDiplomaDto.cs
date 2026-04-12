using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features.Diplomas.DTOS
{
    public class CreateDiplomaDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
    }
}
