using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.GetPublishedDiplomaById
{
    public class GetPublishDiplomaByIdQueryHandler : IRequestHandler<GetPublishDiplomaByIdQuery, DiplomasummaryDTO?>
    {
        private readonly IGeneralRepository<Diploma> _repository;

        //private readonly ICurrentUserService _currentUser;

        public GetPublishDiplomaByIdQueryHandler(IGeneralRepository<Diploma> repository, ICurrentUserService currentUser)
        {
            _repository = repository;

            //_currentUser = currentUser;
        }
        public async Task<DiplomasummaryDTO?> Handle(GetPublishDiplomaByIdQuery request, CancellationToken cancellationToken)
        {

            var query = await _repository.Query()
                        .AsNoTracking()
                        .Select(d => new DiplomasummaryDTO()
                        {
                            Id = d.Id,
                            Title = d.Title,
                            Quizzes = d.Quizzes.Where(q => q.Status == Status.published)
                                        .Select(q => new QuizforDiplomaDTO()
                                        {
                                            Id = q.Id,
                                            Title = q.Title,
                                            DurationMinutes = q.DurationMinutes,
                                            Status = q.Status,
                                            attemptCount = q.Attempts.Count(),
                                            LastScore = q.Attempts
                                                        .Where(a => a.UserId == 2)
                                                        .OrderByDescending(a => a.SubmittedAt)
                                                        .Select(a => (float?)a.score)
                                                        .FirstOrDefault() ?? 0
                                        }).ToList()
                        }).Where(d => d.Id == request.id)
                        .FirstOrDefaultAsync(cancellationToken);

            if (query == null)
                throw new NotFoundException("Diploma not found");

            return query;



        }

    }
}
