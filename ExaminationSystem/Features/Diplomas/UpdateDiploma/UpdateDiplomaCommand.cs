using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.UpdateDiploma
{
    public record UpdateDiplomaCommand (int id, string title, string? description) :IRequest<ApiResponse<UpdateDiplomaDto>>
    {
    }
}   
