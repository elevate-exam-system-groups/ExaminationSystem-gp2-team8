using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Enrollments
{
    public record GetStudentDiplomaEnrollment(int StudentId):IRequest<List<int>>;

    public class GetStudentDiplomaEnrollmentHandler : IRequestHandler<GetStudentDiplomaEnrollment, List<int>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;

        public GetStudentDiplomaEnrollmentHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }

        public async Task<List<int>> Handle(GetStudentDiplomaEnrollment request, CancellationToken cancellationToken)
        {
            var diplomaIds=await _repository.GetAll()
                .Where(e => e.UserId == request.StudentId )
                .Include(e=>e.Diploma)
                .Where(e=>e.Diploma.status==Status.published)
                .Select(e => e.DiplomaId)
                .ToListAsync(cancellationToken);
            return diplomaIds;
        }
    }

}
