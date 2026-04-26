using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Queries.GetQuizDetails
{
    public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, QuizDetailsDTO?>
    {
        private readonly IGeneralRepository<Quiz> _repository;

        public GetQuizByIdQueryHandler(IGeneralRepository<Quiz> repository)
        {
            _repository = repository;
        }
        public async Task<QuizDetailsDTO?> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.GetByIdAsync(request.Id);
            if(quiz==null) return null;
            return new QuizDetailsDTO
            {
                Id = quiz.Id,
                QuizTitle = quiz.Title,
                QuizDuarion=quiz.DurationMinutes
                
            };

            
        }
    }
}
