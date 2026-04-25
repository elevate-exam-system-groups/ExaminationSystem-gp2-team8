using ExaminationSystem.BuildingBlocks.Interfaces;
//using ExaminationSystem.;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.GetAttemptsOverTime
{
    public class GetAttemptsOverTimeQueryHandler : IRequestHandler<GetAttemptsOverTimeQuery, IEnumerable<GetAttemptsOverTimeDTO>>
    {
        private readonly IGeneralRepository<Domain.Entities.Attempts> _repository;

        public GetAttemptsOverTimeQueryHandler(IGeneralRepository<Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<GetAttemptsOverTimeDTO>> Handle(GetAttemptsOverTimeQuery request, CancellationToken cancellationToken)
        {
            var attempts = await _repository.Query().GroupBy(a => a.SubmittedAt.Date)
                .Select(g => new GetAttemptsOverTimeDTO
                {
                    SubmittedAt = g.Key,
                    AttemptCount = g.Count()
                }).ToListAsync();
            return attempts;
           
        }
    }
}
