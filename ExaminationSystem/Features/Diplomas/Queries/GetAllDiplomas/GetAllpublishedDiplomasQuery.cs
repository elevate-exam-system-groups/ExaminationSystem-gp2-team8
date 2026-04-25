using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Enrollments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas
{
    public record GetAllpublishedDiplomasQuery(PaginationParams Params ,int userId) : IRequest<PaginatedResult<DiplomaDTO>>;

}
