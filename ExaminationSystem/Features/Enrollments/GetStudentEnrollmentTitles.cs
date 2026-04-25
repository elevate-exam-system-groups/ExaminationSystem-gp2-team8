using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Students.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Enrollments
{
    public record GetStudentEnrollmentTitles(int StudentId) : IRequest<List<StudentEnrollmentDto>>;

    public class GetStudentEnrollmentTitlesHandler : IRequestHandler<GetStudentEnrollmentTitles, List<StudentEnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;

        public GetStudentEnrollmentTitlesHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }

        public async Task<List<StudentEnrollmentDto>> Handle(GetStudentEnrollmentTitles request, CancellationToken cancellationToken)
        {
            return await _repository.Query()
                .Where(e => e.UserId == request.StudentId)
                .Where(e => e.Diploma.status == Status.published)
                .Select(e => new StudentEnrollmentDto(e.Diploma.Title))
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
