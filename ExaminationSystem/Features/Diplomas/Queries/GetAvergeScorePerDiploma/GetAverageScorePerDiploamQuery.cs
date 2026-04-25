using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAvergeScorePerDiploma
{
    public record GetAverageScorePerDiploamQuery:IRequest<IEnumerable<AverageScorePerDiploamDTO>>;
   
}
