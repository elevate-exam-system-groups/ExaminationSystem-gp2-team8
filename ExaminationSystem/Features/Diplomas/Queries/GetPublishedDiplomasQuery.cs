using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries
{
    public record GetPublishedDiplomasQuery(int Page, int PerPage) :IRequest<PaginatedResult<DiplomapublishedDto>>;

    public class GetPublishedDiplomasQueryHandler : IRequestHandler<GetPublishedDiplomasQuery, PaginatedResult<DiplomapublishedDto>>
    {
        private readonly IGeneralRepository<Diploma> _repository;

        public GetPublishedDiplomasQueryHandler(IGeneralRepository<Diploma> repository)
        {
           _repository = repository;
        }
        public async Task<PaginatedResult<DiplomapublishedDto>> Handle(GetPublishedDiplomasQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query()
                .AsNoTracking()
                .Where(d => d.status == Status.published);

            var totalCount = await query.CountAsync(cancellationToken);

            var diplomaDtos = await query
                .OrderBy(d => d.Id)
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(d => new DiplomapublishedDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description ?? string.Empty,
                    QuizCount = d.Quizzes.Count
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResult<DiplomapublishedDto>(diplomaDtos, totalCount, request.Page, request.PerPage);
        }
    }

}
