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

            var groupedAttempts = await query
                .GroupBy(a => new
                {
                    a.SubmittedAt.Year,
                    a.SubmittedAt.Month,
                    a.SubmittedAt.Day
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.Day,
                    AttemptCount = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ThenBy(x => x.Day)
                .ToListAsync();

            return groupedAttempts.Select(x => new GetAttemptsOverTimeDTO
            {
                SubmittedAt = new DateTime(x.Year, x.Month, x.Day, 0, 0, 0, DateTimeKind.Utc),
                AttemptCount = x.AttemptCount
            });
        }
    }
}
