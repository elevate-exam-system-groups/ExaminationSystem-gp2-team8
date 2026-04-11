using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Enrollments;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries
{
    public record GetPublishDiplomaByIdQuery(int id) : IRequest<ApiResponse<DiplomasummaryDTO?>>;


    public class GetPublishDiplomaByIdQueryHandler : IRequestHandler<GetPublishDiplomaByIdQuery, ApiResponse<DiplomasummaryDTO?>>
    {
        private readonly IGeneralRepository<Diploma> _repository;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public GetPublishDiplomaByIdQueryHandler(IGeneralRepository<Diploma> repository, IMediator mediator, ICurrentUserService currentUser)
        {
            _repository = repository;
            _mediator = mediator;
            _currentUser = currentUser;
        }
        public async Task<ApiResponse<DiplomasummaryDTO?>> Handle(GetPublishDiplomaByIdQuery request, CancellationToken cancellationToken)
        {


            //wait till student Auth
            var enrolled = await _mediator.Send(new GetStudentDiplomaEnrollment(_currentUser.UserId));
            if (!enrolled.Contains(request.id))
            {
                return ApiResponse<DiplomasummaryDTO?>.FailureResponse("Student not enrolled in diploma", "FORRBIDN");
            }

            var diploma = _repository.GetAll()
             .Include(d => d.Quizzes)
             .ThenInclude(q => q.Attempts)
             .Where(d => d.Id == request.id && d.status==Status.pulished)
             .FirstOrDefault();
            if (diploma == null)
            {
                return ApiResponse<DiplomasummaryDTO?>.FailureResponse("diploma is not found or puplished", "NOT_FOUND");

            }



            var diplomaDTO = new DiplomasummaryDTO()
            {
                Id=diploma!.Id,
                Title=diploma.Title,
                Quizzes=diploma.Quizzes
                .Where(q => q.Status == Status.pulished)
                .Select(q=> new QuizforDiplomaDTO()
                {
                    Id=q.Id,
                    Title = q.Title,
                    DurationMinutes=q.DurationMinutes,
                    Status=q.Status,
                    attemptCount=q.Attempts.Count(),
                    LastScore=q.Attempts.OrderByDescending(a=>a.score).FirstOrDefault()?.score ??0,
                }).ToList()
            };

            return ApiResponse<DiplomasummaryDTO?>.SuccessResponse(diplomaDTO);


        }
    }
}
