using MediatR;

namespace ExaminationSystem.Features.Diplomas.DeleteDiploma
{
    public record DeleteDiplomaCommand(int id) : IRequest<bool>
    {
    }
}
