using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas
{
    public class GetAllpublishedDiplomasQueryHandler:IRequestHandler<GetAllpublishedDiplomasQuery, PaginatedResult<DiplomaDTO>>
    {
    
        IGeneralRepository<Diploma> _diplomaRepository;

        public GetAllpublishedDiplomasQueryHandler( IGeneralRepository<Diploma> diplomaRepository)
        {
        
            _diplomaRepository = diplomaRepository;
        }

        public async Task<PaginatedResult<DiplomaDTO>> Handle(GetAllpublishedDiplomasQuery request, CancellationToken cancellationToken)
        {
       
            var query = _diplomaRepository.Query().AsNoTracking()
                      .Where(d => d.status == Status.published)
                      .Select(diploma => new DiplomaDTO()
                      {
                          Id = diploma.Id,
                          Title = diploma.Title,
                          Description = diploma.Description ?? "",
                          QuizCount = diploma.QuizCount,
                          StudentProgress = new StudentProgressDto
                          {

                              CompletedQuizzes = diploma.Quizzes.Count(q => q.Attempts.Any(a => a.UserId == request.userId && a.Attempt == AttemptStatus.Passed)),
                              TotalAttempts = diploma.Quizzes
                                                       .SelectMany(q => q.Attempts).Count(a => a.UserId == request.userId),
                             

                          }

                      });

            if (query == null)
                throw new NotFoundException("");

            return await query.ApplyPagination(request.Params.Page, request.Params.PerPage);
        }
    }
}
