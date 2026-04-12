using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.DeleteDiploma
{
    public record DeleteDiplomaCommand(int id) : IRequest<ApiResponse<bool>>
    {
    }
}
