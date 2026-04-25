using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.UnpublishQuiz
{
    public class UnpublishQuizCommandHandler : IRequestHandler<UnpublishQuizCommand, bool>
    {
        private readonly IGeneralRepository<Domain.Entities.Attempts> _repository;
        private readonly IGeneralRepository<Quiz> _quizRepository;

        public UnpublishQuizCommandHandler(IGeneralRepository<Domain.Entities.Attempts> attemptRepository, IGeneralRepository<Quiz> quizRepository)
        {
            _repository = attemptRepository;
            _quizRepository = quizRepository;
        }
        public async Task<bool> Handle(UnpublishQuizCommand request, CancellationToken cancellationToken)
        {
            var hasInProgressAttempts = await _repository.Query()
                .AsNoTracking()
                .AnyAsync(x => x.QuizId == request.id && x.Attempt == AttemptStatus.InProgress);

            if (hasInProgressAttempts)
                throw new ConflictException("Can't unpublish quiz while in-progress attempts exist");

            var quiz = await _quizRepository.GetByIdAsync(request.id);
            if (quiz is null)
                throw new NotFoundException("Quiz is not found");

            quiz.Status = Status.Draft;
            _quizRepository.Update(quiz);
            await _quizRepository.SaveChangesAsync();

            return true;
        }
    }
}
