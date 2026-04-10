using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Queries
{
    public record GetAllDiplomasQuery : IRequest<PaginatedResult<DiplomaDTO>>;

    public class GetAllDiplomasQueryHandler : IRequestHandler<GetAllDiplomasQuery, PaginatedResult<DiplomaDTO>>
    {
        public Task<PaginatedResult<DiplomaDTO>> Handle(GetAllDiplomasQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
