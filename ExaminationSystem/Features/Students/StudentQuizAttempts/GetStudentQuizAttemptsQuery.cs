using ExaminationSystem.Features.Students.DTOs;
using MediatR;

namespace ExaminationSystem.Features.Students.StudentQuizAttempts
{
    public record GetStudentQuizAttemptsQuery(int studentId, List<int> DiplomasIds) : IRequest<IEnumerable<StudentQuizAttemptsDto>>
    {
    }
}
