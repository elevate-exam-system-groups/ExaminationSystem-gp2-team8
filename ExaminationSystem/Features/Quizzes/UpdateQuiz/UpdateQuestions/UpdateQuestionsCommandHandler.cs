using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.UpdateQuiz.UpdateQuestions
{
    public class UpdateQuestionsCommandHandler : IRequestHandler<UpdateQuestionsCommand, bool>
    {
        private readonly IGeneralRepository<Question> _questionRepository;
        private readonly IGeneralRepository<Options> _optionRepository;

        public UpdateQuestionsCommandHandler(
            IGeneralRepository<Question> questionRepository,
            IGeneralRepository<Options> optionRepository)
        {
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
        }
        public async Task<bool> Handle(UpdateQuestionsCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var question = await _questionRepository.GetByIdAsync(request.questionId)
                ?? throw new NotFoundException("Question Not Found");

            question.QuestionText = request.dto.QuestionText.Trim();
            question.Explanation = request.dto.Explanation?.Trim();

            var existingOptions = await _optionRepository.Query()
                .Where(option => option.QuestionId == question.Id)
                .ToListAsync(cancellationToken);

            foreach (var existingOption in existingOptions)
            {
                _optionRepository.Delete(existingOption);
            }

            foreach (var option in request.dto.Options)
            {
                await _optionRepository.AddAsync(new Options
                {
                    QuestionId = question.Id,
                    OptionText = option.OptionText.Trim(),
                    IsCorrect = option.IsCorrect
                });
            }

            _questionRepository.Update(question);
            await _questionRepository.SaveChangesAsync();

            return true;
        }

        private async Task ValidateRequestAsync(UpdateQuestionsCommand request, CancellationToken cancellationToken)
        {
            var questionExists = await _questionRepository.Query()
                .AsNoTracking()
                .AnyAsync(q => q.Id == request.questionId, cancellationToken);

            if (!questionExists)
            {
                throw new NotFoundException("Question Not Found");
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
                throw new ValidationException("At least 2 options required");
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
