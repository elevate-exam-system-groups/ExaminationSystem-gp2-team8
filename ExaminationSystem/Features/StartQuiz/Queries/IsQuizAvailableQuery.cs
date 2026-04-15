using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using MediatR;

namespace ExaminationSystem.Features.StartQuiz.Queries
{
    public record IsQuizAvailableQuery(int QuizId) : IRequest<bool>;

    public class IsQuizAvailableQueryHandler : IRequestHandler<IsQuizAvailableQuery, bool>
    {
        private readonly IGenericRepository<Quiz> _quizRepository;

        public IsQuizAvailableQueryHandler(IGenericRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<bool> Handle(IsQuizAvailableQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

            return quiz != null
                   && !quiz.IsDeleted
                   && quiz.Status == Status.Published;
        }
    }
}