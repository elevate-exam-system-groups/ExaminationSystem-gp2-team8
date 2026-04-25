using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Queries.GetPassRateByQuiz
{
    public class GetPassRateByQuizQuery: IRequest<IEnumerable<PassRateByQuizDTO>>;
    
}
