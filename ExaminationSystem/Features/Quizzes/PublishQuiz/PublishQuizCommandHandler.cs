using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.PublishQuiz
{
    public class PublishQuizCommandHandler : IRequestHandler<PublishQuizCommand, PublishQuizDto>
    {
        private readonly IGeneralRepository<Quiz> _repository;

        public PublishQuizCommandHandler(IGeneralRepository<Quiz> repository)
        {
            _repository = repository;
        }
        public async Task<PublishQuizDto> Handle(PublishQuizCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var quiz = await _repository.GetByIdAsync(request.QuizId);

            quiz!.Status = Status.published;
            _repository.Update(quiz);
            await _repository.SaveChangesAsync();

            return new PublishQuizDto
            {
                Id = request.QuizId,
                Status = Status.published.ToString()
            };
        }
        private async Task ValidateRequestAsync(PublishQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.Query()
                .Where(q => q.Id == request.QuizId)
                .Select(q => new QuizPublishValidationData(
                    q.Status,
                    q.Questions.Select(question => new QuestionValidationData(
                        question.Options.Count,
                        question.Options.Count(o => o.IsCorrect)))
                    .ToList()))
                .FirstOrDefaultAsync(cancellationToken);

            if (quiz is null)
                throw new NotFoundException("Quiz is not found");

            if (quiz.Questions.Count == 0)
                throw new ValidationException("Quiz must have at least one question");

            if (quiz.Status == Status.published)
                throw new ConflictException("Quiz is already published");

            if (quiz.Questions.Any(q => q.OptionCount < 2 || q.CorrectOptionCount != 1))
                throw new ValidationException("All questions must have valid options before publishing");
        }

        private sealed record QuizPublishValidationData(
            Status Status,
            List<QuestionValidationData> Questions);

        private sealed record QuestionValidationData(
            int OptionCount,
            int CorrectOptionCount);
    }

}
