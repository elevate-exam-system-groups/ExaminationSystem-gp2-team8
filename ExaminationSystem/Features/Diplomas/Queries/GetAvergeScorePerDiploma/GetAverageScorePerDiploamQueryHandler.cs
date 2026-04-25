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
        public async Task<IEnumerable<AverageScorePerDiploamDTO>> Handle(GetAverageScorePerDiploamQuery request, CancellationToken cancellationToken)
        {
            var diplomas = await _repository.Query()
                            .Select(d => new AverageScorePerDiploamDTO
                             {
                                    DiplomaId = d.Id,
                                    DiplomaTitle = d.Title,
                                    AverageScore = d.Quizzes
                                    .SelectMany(q => q.Attempts)
                                    .Select(a => (float?)a.score)
                                    .Average() ?? 0

                             }).ToListAsync(cancellationToken);
          
            return diplomas;
        }
    }
}
