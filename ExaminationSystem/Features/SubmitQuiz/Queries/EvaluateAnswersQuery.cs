using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record EvaluateAnswersQuery(
       List<QuestionDto> Questions,
       Dictionary<int, List<OptionsDto>> OptionsMap,
       ICollection<QuizAnswerDto> Answers
   ) : IRequest<Result<EvaluateQuizResultDto>>;

    public class EvaluateAnswersQueryHandler
        : IRequestHandler<EvaluateAnswersQuery, Result<EvaluateQuizResultDto>>
    {
        public Task<Result<EvaluateQuizResultDto>> Handle(
            EvaluateAnswersQuery request,
            CancellationToken cancellationToken)
        {
            int correct = 0;
            int wrong = 0;

            foreach (var answer in request.Answers)
            {
                var questionExists = request.Questions
                    .Any(q => q.QuestionId == answer.QuestionId);

                if (!questionExists)
                {
                    wrong++;
                    continue;
                }

                if (!request.OptionsMap.TryGetValue(answer.QuestionId, out var options))
                {
                    wrong++;
                    continue;
                }

                var correctOption = options.FirstOrDefault(o => o.IsCorrect);

                if (correctOption != null && correctOption.OptionId == answer.SelectedOptionId)
                    correct++;
                else
                    wrong++;
            }

            int total = request.Questions.Count;
            double percentage = total == 0 ? 0 : (double)correct / total * 100;

            return Task.FromResult(Result<EvaluateQuizResultDto>.Success(new EvaluateQuizResultDto
            {
                CorrectAnswers = correct,
                WrongAnswers = wrong,
                Percentage = percentage
            }));
        }
    }
}
