using ExaminationSystem.Features.Analytics.DTO;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAvergeScorePerDiploma
{
    public record GetAverageScorePerDiploamQuery(FiltersElement Filters):IRequest<IEnumerable<AverageScorePerDiploamDTO>>;
   
}
