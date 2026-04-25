using ExaminationSystem.Features.AnswerQuestion.DTOs;
using MediatR;

namespace ExaminationSystem.Features.AnswerQuestion.GetTopFailedQuestion
{
    public record GetTopFailedQuestionQuery:IRequest<IEnumerable<TopFailedQuestionDTO>>;
   
}
