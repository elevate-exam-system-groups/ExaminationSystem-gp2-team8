using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using MediatR;

namespace ExaminationSystem.Features.StartQuiz.Queries
{
    public record IsQuizAvailableQuery(int QuizId) : IRequest<Result<bool>>;

    public class IsQuizAvailableQueryHandler : IRequestHandler<IsQuizAvailableQuery, Result<bool>>
    {
        private readonly IGenericRepository<Quiz> _quizRepository;

        public IsQuizAvailableQueryHandler(IGenericRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<Result<bool>> Handle(IsQuizAvailableQuery request, CancellationToken cancellationToken)
        {
            var exists = await _quizRepository.ExistsAsync(q => q.Id == request.QuizId && !q.IsDeleted && q.Status == Status.Published);
            return Result<bool>.Success(exists);
        }
    }
}