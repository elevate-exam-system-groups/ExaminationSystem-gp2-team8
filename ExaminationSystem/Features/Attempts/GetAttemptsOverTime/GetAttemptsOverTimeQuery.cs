using ExaminationSystem.Features.Analytics.DTO;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Attempts.GetAttemptsOverTime
{
    public record GetAttemptsOverTimeQuery(FiltersElement Filters) : IRequest<IEnumerable<GetAttemptsOverTimeDTO>>;
   
}
