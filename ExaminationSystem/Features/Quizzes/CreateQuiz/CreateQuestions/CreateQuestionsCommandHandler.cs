using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz.CreateQuestions
{
    public class CreateQuestionsCommandHandler : IRequestHandler<CreateQuestionsCommand, int>
    {
        private readonly IGeneralRepository<Quiz> _quizRepository;
        private readonly IGeneralRepository<Question> _questionRepository;

        public CreateQuestionsCommandHandler(
            IGeneralRepository<Quiz> quizRepository,
            IGeneralRepository<Question> questionRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
        }
        public async Task<int> Handle(CreateQuestionsCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var question = new Question
            {
                QuestionText = request.dto.QuestionText.Trim(),
                Explanation = request.dto.Explanation?.Trim(),
                QuizId = request.quizId,
            };

            foreach (var option in request.dto.Options)
            {
                question.Options.Add(new Options
                {
                    OptionText = option.OptionText.Trim(),
                    IsCorrect = option.IsCorrect,
                });
            }

            await _questionRepository.AddAsync(question);
            await _questionRepository.SaveChangesAsync();

            return question.Id;
        }

        private async Task ValidateRequestAsync(CreateQuestionsCommand request, CancellationToken cancellationToken)
        {
            var quizExists = await _quizRepository.Query()
                .AsNoTracking()
                .AnyAsync(q => q.Id == request.quizId, cancellationToken);

            if (!quizExists)
            {
                throw new NotFoundException("Quiz Not Found");
            }

            ValidateQuestion(request.dto);
        }

        private static void ValidateQuestion(CreateQuestionsforQuizDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.QuestionText))
            {
                throw new ValidationException("Question text is required");
            }

            if (dto.Options.Count < 2)
            {
                throw new ValidationException("Question must have at least 2 options");
            }

            if (dto.Options.Count(option => option.IsCorrect) != 1)
            {
                throw new ValidationException("Exactly one correct option required");
            }

            if (dto.Options.Any(option => string.IsNullOrWhiteSpace(option.OptionText)))
            {
                throw new ValidationException("Option text is required");
            }
        }
    }
}
