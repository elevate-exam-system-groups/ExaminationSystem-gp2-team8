using ExaminationSystem.Features.Students.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Students.OverallStudentStats
{
    public record GetOverallStudentStatsQuery(int studentId) : IRequest<StudentStatsDto>;
}
