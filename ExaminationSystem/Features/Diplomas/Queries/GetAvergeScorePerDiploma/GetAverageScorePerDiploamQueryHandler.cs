using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAvergeScorePerDiploma
{
    public class GetAverageScorePerDiploamQueryHandler : IRequestHandler<GetAverageScorePerDiploamQuery, IEnumerable<AverageScorePerDiploamDTO>>
    {
        private readonly IGeneralRepository<Diploma> _repository;


        public GetAverageScorePerDiploamQueryHandler(IGeneralRepository<Diploma> repository
            )
        {
            _repository = repository;

        }
        public async Task<IEnumerable<AverageScorePerDiploamDTO>> Handle(
             GetAverageScorePerDiploamQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query().AsNoTracking();

            // Filter which diplomas to return
            if (request.Filters.DiplomaId.HasValue)
                query = query.Where(d => d.Id == request.Filters.DiplomaId.Value);

            var from = request.Filters.From;
            var to = request.Filters.To;


            return await query
                .Select(d => new AverageScorePerDiploamDTO
                {
                    DiplomaId = d.Id,
                    DiplomaTitle = d.Title,
                    AverageScore = d.Quizzes
                        .SelectMany(q => q.Attempts)
                        .Where(a =>
                            (!from.HasValue || a.SubmittedAt >= from.Value) &&
                            (!to.HasValue || a.SubmittedAt <= to.Value))
                        .Select(a => (float?)a.score)
                        .Average() ?? 0
                })
                .ToListAsync(cancellationToken);
        }
    }
}
