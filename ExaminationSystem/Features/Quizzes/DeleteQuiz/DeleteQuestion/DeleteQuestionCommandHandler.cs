using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.DeleteQuiz.DeleteQuestion
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, bool>
    {
        private readonly IGeneralRepository<Question> _repository;

        public DeleteQuestionCommandHandler(IGeneralRepository<Question> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var question = await _repository.GetByIdAsync(request.questionId)
                ?? throw new NotFoundException("Question Not Found");

            _repository.Delete(question);
            await _repository.SaveChangesAsync();

            return true;
        }

        private async Task ValidateRequestAsync(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var questionState = await _repository.Query()
                .AsNoTracking()
                .Where(q => q.Id == request.questionId)
                .Select(q => new QuestionDeleteState(q.Id, q.Quiz.Status))
                .FirstOrDefaultAsync(cancellationToken);

            if (questionState is null)
            {
                throw new NotFoundException("Question Not Found");
            }

            if (questionState.QuizStatus == Status.published)
            {
                throw new ConflictException("Cannot delete question while quiz is published");
            }
        }

        private sealed record QuestionDeleteState(int Id, Status QuizStatus);
    }
}
