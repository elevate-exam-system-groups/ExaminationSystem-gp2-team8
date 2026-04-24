using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Attempts.GetStudentByQuizIdandStudntId
{
    public record GetStudentByQuizIdandStudntIdQuery(PaginationParams Params,int quizId,int studentId):IRequest<PaginatedResult<FilteredAttemptsDTO>>;
   
}
