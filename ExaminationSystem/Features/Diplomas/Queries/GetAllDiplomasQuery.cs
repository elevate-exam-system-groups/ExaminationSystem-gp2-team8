using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Enrollments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries
{
    public record GetAllDiplomasQuery(int Page , int PerPage ) : IRequest<PaginatedResult<DiplomaDTO>>;

    public class GetAllDiplomasQueryHandler : IRequestHandler<GetAllDiplomasQuery, PaginatedResult<DiplomaDTO>>
    {

        private readonly ICurrentUserService _currentUser;
        private readonly IMediator _mediator;

        public GetAllDiplomasQueryHandler(ICurrentUserService currentUser, IMediator mediator)
        {

            _currentUser = currentUser;
            _mediator = mediator;
        }
        public async Task<PaginatedResult<DiplomaDTO>> Handle(GetAllDiplomasQuery request, CancellationToken cancellationToken)
        {

            //get all enrolled diploma ids for the current user
            var enrolledDiplomaIds = await _mediator.Send(new GetStudentDiplomaEnrollment(_currentUser.UserId), cancellationToken);

            //get all diplomas with quiz attempts for the enrolled diploma ids
            var diplomawithQuizAttempts = await _mediator.Send(new GetDiplomasWithQuizAttempts(enrolledDiplomaIds), cancellationToken);

            //get all published diplomas with pagination
            var paginatedDiplomas = await _mediator.Send(
                                    new GetPublishedDiplomasQuery(request.Page, request.PerPage), cancellationToken);

            //join the paginated diplomas with the quiz attempts to get the final result

            var diplomaDTOS = (from diploma in paginatedDiplomas.Data
                               join attempts in diplomawithQuizAttempts on diploma.Id equals attempts.Id into attemptsGroup
                               from attempts in attemptsGroup.DefaultIfEmpty()
                               select new DiplomaDTO()
                               {
                                   Id = diploma.Id,
                                   Title = diploma.Title,
                                   Description = diploma.Description,
                                   QuizCount = diploma.QuizCount,
                                   StudentProgress = new StudentProgressDto
                                   {
                                       CompletedQuizzes = attempts?.Quizzes
                               .Count(q => q.Attempts.Any(a => a.Status == AttemptStatus.Passed.ToString())) ?? 0,
                                       TotalAttempts = attempts?.Quizzes
                               .SelectMany(q => q.Attempts).Count() ?? 0,
                                       AverageScore = attempts?.Quizzes.SelectMany(q => q.Attempts).Any() == true
                               ? Math.Round(attempts.Quizzes.SelectMany(q => q.Attempts).Average(a => a.Score), 2)
                               : 0
                                   }

                               }).ToList();
            return new PaginatedResult<DiplomaDTO>(diplomaDTOS, paginatedDiplomas.TotalCount, request.Page, request.PerPage);
        }
    }

}
