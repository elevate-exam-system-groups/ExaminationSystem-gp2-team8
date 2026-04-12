using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Attempts.GetAttemptDetails
{
    public record GetStudentAttemptDetailsQuery(int StudentId, int AttemptId) : IRequest<AttemptResultDetailDto>;
}
