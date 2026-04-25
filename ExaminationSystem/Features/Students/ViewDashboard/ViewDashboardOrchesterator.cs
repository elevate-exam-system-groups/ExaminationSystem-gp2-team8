using ExaminationSystem.Features.Enrollments;
using ExaminationSystem.Features.Students.DTOs;
using ExaminationSystem.Features.Students.OverallStudentStats;
using ExaminationSystem.Features.Students.StudentQuizAttempts;
using MediatR;

namespace ExaminationSystem.Features.Students.ViewDashboard
{
    public class ViewDashboardOrchesterator : IRequestHandler<ViewDashboardQuery, GetOverallStatsDto>
    {
        private readonly IMediator _mediator;

        public ViewDashboardOrchesterator(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<GetOverallStatsDto> Handle(ViewDashboardQuery request, CancellationToken cancellationToken)
        {
            var dipIDs = await _mediator.Send(new GetStudentDiplomaEnrollment(request.studentId), cancellationToken);

            var studentEnrollments = await _mediator.Send(new GetStudentEnrollmentTitles(request.studentId), cancellationToken);

            var quizAttempts = await _mediator.Send(new GetStudentQuizAttemptsQuery(request.studentId, dipIDs));

            var overallStats = await _mediator.Send(new GetOverallStudentStatsQuery(request.studentId));


            return new GetOverallStatsDto
            {
                StudentEnrollments = studentEnrollments,
                QuizAttempts = quizAttempts,
                OverallStats = overallStats
            };

        }
    }
}
