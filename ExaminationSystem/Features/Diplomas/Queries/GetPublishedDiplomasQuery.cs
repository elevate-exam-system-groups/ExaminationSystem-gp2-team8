using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var query = _repository.GetAll()
                .Include(d => d.Quizzes)
                .Where(d => d.status == Status.pulished);
               

            var totalCount = await query.CountAsync(cancellationToken);


            var diplomas=await query.ToListAsync(cancellationToken);

            var diplomaDtos = diplomas.Select(d => new DiplomapublishedDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                QuizCount = d.Quizzes.Count
            }).ToList();

            return new PaginatedResult<DiplomapublishedDto>(diplomaDtos, totalCount, request.Page, request.PerPage);
        }
    }

}
