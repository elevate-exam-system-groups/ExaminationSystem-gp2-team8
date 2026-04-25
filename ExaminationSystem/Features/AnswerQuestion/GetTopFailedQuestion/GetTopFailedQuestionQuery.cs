using ExaminationSystem.Features.Analytics.DTO;
using ExaminationSystem.Features.AnswerQuestion.DTOs;
using MediatR;

namespace ExaminationSystem.Features.AnswerQuestion.GetTopFailedQuestion
{
    public record GetTopFailedQuestionQuery(FiltersElement Filters):IRequest<IEnumerable<TopFailedQuestionDTO>>;
   
}
