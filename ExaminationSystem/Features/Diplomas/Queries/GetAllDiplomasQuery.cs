using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries
{
    public record GetAllDiplomasQuery(int Page = 1, int PerPage = 10) : IRequest<PaginatedResult<DiplomaDTO>>;

    public class GetAllDiplomasQueryHandler : IRequestHandler<GetAllDiplomasQuery, PaginatedResult<DiplomaDTO>>
    {
        private readonly IGeneralRepository<Diploma> _repository;

        public GetAllDiplomasQueryHandler(IGeneralRepository<Diploma> repository)
        {
            _repository = repository;
        }
        public async Task<PaginatedResult<DiplomaDTO>> Handle(GetAllDiplomasQuery request, CancellationToken cancellationToken)
        {
            var query=_repository.GetAll()
                .Include(d => d.Quizzes)
                .Where(d => d.status == Status.pulished);

            var totalCount = await query.CountAsync(cancellationToken);

            var diplomas = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .ToListAsync(cancellationToken);

            var diplomasDTOs = diplomas.Select(d => new DiplomaDTO
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                QuizCount = d.Quizzes.Count,
                StudentProgress = 0 // This should be calculated based on the student's progress in the quizzes
            }).ToList();

            return new PaginatedResult<DiplomaDTO>(diplomasDTOs,totalCount,request.Page, request.PerPage);
   

        }
    }

}
