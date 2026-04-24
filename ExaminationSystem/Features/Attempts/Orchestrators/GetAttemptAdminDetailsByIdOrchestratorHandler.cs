using ExaminationSystem.Features.AnswerQuestion.GetQuestionAnswerDetails;
using ExaminationSystem.Features.Attempts.DTOs;
using ExaminationSystem.Features.Attempts.GetAttemptDetailsForAdmin;
using ExaminationSystem.Features.Quizzes.GetQuizDetails;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Orchestrators
{
    public class GetAttemptAdminDetailsByIdOrchestratorHandler : IRequestHandler<GetAttemptAdminDetailsByIdOrchestrator, AttemptForAdminDetailsDTO>
    {
        private readonly IMediator _mediator;

        public GetAttemptAdminDetailsByIdOrchestratorHandler(IMediator mediator)
        {
           _mediator = mediator;
        }
        public async Task<AttemptForAdminDetailsDTO> Handle(GetAttemptAdminDetailsByIdOrchestrator request, CancellationToken cancellationToken)
        {
            var attemptDetails =await _mediator.Send(new GetDetailsAttemptByIdForAdminQuery(request.attemptId), cancellationToken);

            var Quiz= await _mediator.Send(new GetQuizByIdQuery(attemptDetails!.QuizId), cancellationToken);

            var questions=await _mediator.Send(new GetAnswerWithAnswerDetailsQuery(request.attemptId), cancellationToken);
            //return attemptDetails;

            return  new AttemptForAdminDetailsDTO()
            {
                attemptedId=attemptDetails!.attemptedId,
                QuizId=attemptDetails.QuizId,
                QuizTitle=Quiz!.QuizTitle,
                QuizDuration=Quiz.QuizDuarion,
                studentId=attemptDetails.studentId,
                score=attemptDetails.score,
                submittedAt=attemptDetails.submittedAt,
                status=attemptDetails.status,
                Questions=questions
            };
        }
    }
}
