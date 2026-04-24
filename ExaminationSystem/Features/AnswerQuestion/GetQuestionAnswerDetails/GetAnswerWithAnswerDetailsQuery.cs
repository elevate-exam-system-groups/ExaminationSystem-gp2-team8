using MediatR;

namespace ExaminationSystem.Features.AnswerQuestion.GetQuestionAnswerDetails
{
    public record GetAnswerWithAnswerDetailsQuery(int attemptId):IRequest<IEnumerable<GetAnswerDetailsDTO>>;

}
