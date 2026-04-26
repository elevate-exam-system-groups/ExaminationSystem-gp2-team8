using ExaminationSystem.Features.Analytics.DTO;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Queries.GetPassRateByQuiz
{
    public record GetPassRateByQuizQuery(FiltersElement Filters) : IRequest<IEnumerable<PassRateByQuizDTO>>;
    
}
