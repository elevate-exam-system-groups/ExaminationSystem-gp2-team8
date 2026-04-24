using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Orchestrators
{
    public record GetAttemptAdminDetailsByIdOrchestrator(int attemptId) : IRequest<AttemptForAdminDetailsDTO>;
   
}
