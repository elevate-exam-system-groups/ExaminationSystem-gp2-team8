using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record CalculateQuizResultQuery(
        EvaluateQuizResultDto Evaluation
    ) : IRequest<Result<SubmitQuizResultDto>>;

    public class CalculateQuizResultQueryHandler
        : IRequestHandler<CalculateQuizResultQuery, Result<SubmitQuizResultDto>>
    {
        public Task<Result<SubmitQuizResultDto>> Handle(
            CalculateQuizResultQuery request,
            CancellationToken cancellationToken)
        {
            var evaluation = request.Evaluation;

            if (evaluation == null)
                return Task.FromResult(
                    Result<SubmitQuizResultDto>.Fail("Invalid evaluation"));

            var score = (int)evaluation.Percentage;

            var result = new SubmitQuizResultDto
            {
                Score = score,
                IsPassed = evaluation.IsPassed,
                AttemptId = 0
            };

            return Task.FromResult(
                Result<SubmitQuizResultDto>.Success(result));
        }
    }
}