using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.GetQuizDetails
{
    public record GetQuizByIdQuery(int Id) : IRequest<QuizDetailsDTO?>;
    
}
