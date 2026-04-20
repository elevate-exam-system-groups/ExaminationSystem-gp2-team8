using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.CreateDiploma
{
    public record CreateDiplomaCommand(string Title, string? Descreption) : IRequest<CreateDiplomaDto>
    {
    }
}
