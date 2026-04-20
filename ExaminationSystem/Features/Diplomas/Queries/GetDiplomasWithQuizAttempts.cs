using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Attempt.DTOS;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries
{
    public record GetDiplomasWithQuizAttempts(List<int>DiplomasIds) : IRequest<IEnumerable<DiplomaQuizAttemptsDto>>;
   

    public class GetDiplomasWithQuizAttemptsHandler : IRequestHandler<GetDiplomasWithQuizAttempts, IEnumerable<DiplomaQuizAttemptsDto>>
    {
        private readonly IGeneralRepository<Diploma> _repository;
        public GetDiplomasWithQuizAttemptsHandler(IGeneralRepository<Diploma> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<DiplomaQuizAttemptsDto>> Handle(GetDiplomasWithQuizAttempts request, CancellationToken cancellationToken)
        {
            var diplomas = await _repository.GetAll()
                           .Where(d => request.DiplomasIds.Contains(d.Id))
                           .Include(d=>d.Quizzes)
                           .ThenInclude(q=>q.Attempts)
                           .ToListAsync(cancellationToken);

            var diplomasWithAttempts = diplomas.Select(d => new DiplomaQuizAttemptsDto
            {
                Id = d.Id,
                Name = d.Title,
                Description = d.Description,
                QuizCount = d.Quizzes.Count,
                Quizzes = d.Quizzes.Select(q => new QuizAttemptsDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    TotalAttempts = q.Attempts.Count,
                    Attempts = q.Attempts.Select(a => new AttemptDto
                    {
                      
                        Score = a.score,
                        Status = a.Attempt.ToString()
                    }).ToList()
                }).ToList()
            });

            return diplomasWithAttempts;
        }
    }
}
