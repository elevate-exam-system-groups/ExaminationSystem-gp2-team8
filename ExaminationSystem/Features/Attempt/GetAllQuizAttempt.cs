using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using MediatR;

namespace ExaminationSystem.Features.Attempt
{
    public record GetQuizAttempts(int StudentId, int QuizId) : IRequest<IEnumerable<Attempts>>;


    public class GetQuizAttemptsHandler : IRequestHandler<GetQuizAttempts, IEnumerable<Attempts>>
    {
        public Task<IEnumerable<Attempts>> Handle(GetQuizAttempts request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
