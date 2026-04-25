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
        public async Task<IEnumerable<GetAttemptsOverTimeDTO>> Handle(
    GetAttemptsOverTimeQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query().AsNoTracking();

            if (request.Filters.DiplomaId.HasValue)
                query = query.Where(a => a.Quiz.DiplomaId == request.Filters.DiplomaId.Value);

            if (request.Filters.From.HasValue)
                query = query.Where(a => a.SubmittedAt >= request.Filters.From.Value);

            if (request.Filters.To.HasValue)
                query = query.Where(a => a.SubmittedAt <= request.Filters.To.Value);

            return await query
                .GroupBy(a => a.SubmittedAt.Date)
                .Select(g => new GetAttemptsOverTimeDTO
                {
                    SubmittedAt = g.Key,
                    AttemptCount = g.Count()
                })
                .OrderBy(x => x.SubmittedAt)   // consistent time-series order
                .ToListAsync(cancellationToken);
        }
    }
}
