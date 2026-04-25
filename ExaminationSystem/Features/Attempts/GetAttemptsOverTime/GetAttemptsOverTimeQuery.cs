using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Attempts.GetAttemptsOverTime
{
    public record GetAttemptsOverTimeQuery : IRequest<IEnumerable<GetAttemptsOverTimeDTO>>;
   
}
