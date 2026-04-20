using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz
{
    public class CreateQuizOrchestrator : IRequestHandler<CreateQuizCommand, CreateQuizResponse>
    {
        private readonly IGeneralRepository<Quiz> _quizRepository;
        private readonly IGeneralRepository<Diploma> _diplomaRepository;

        public CreateQuizOrchestrator(
            IGeneralRepository<Quiz> quizRepository,
            IGeneralRepository<Diploma> diplomaRepository)
        {
            _quizRepository = quizRepository;
            _diplomaRepository = diplomaRepository;
        }
        public async Task<CreateQuizResponse> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var quiz = new Quiz
            {
                Title = request.dto.Title.Trim(),
                DiplomaId = request.dto.DiplomaId,
                DurationMinutes = request.dto.DurationMinutes,
                Instructions = request.dto.Instructions?.Trim(),
                MaxAttempts = request.dto.MaxAttempts,
                PassScore = request.dto.PassScore,
                Status = Domain.Enums.Status.Draft,
            };

            await _quizRepository.AddAsync(quiz);
            await _quizRepository.SaveChangesAsync();

            return MapToResponse(quiz);
        }

        private async Task ValidateRequestAsync(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            ValidateQuiz(request.dto);

            var diplomaExists = await _diplomaRepository.Query()
                .AsNoTracking()
                .AnyAsync(d => d.Id == request.dto.DiplomaId, cancellationToken);

            if (!diplomaExists)
            {
                throw new NotFoundException("Diploma Not Found");
            }
        }

        private static void ValidateQuiz(CreateQuizDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ValidationException("Title is required");
            }

            if (dto.DurationMinutes <= 0)
            {
                throw new ValidationException("DurationMinutes must be a positive integer");
            }

            if (dto.PassScore is < 0 or > 100)
            {
                throw new ValidationException("PassScore must be between 0 and 100");
            }
        }

        private static CreateQuizResponse MapToResponse(Quiz quiz)
        {
            return new CreateQuizResponse(
                quiz.Id,
                quiz.Title,
                quiz.DiplomaId,
                quiz.DurationMinutes,
                quiz.PassScore,
                quiz.MaxAttempts,
                quiz.Instructions,
                quiz.Status.ToString()
            );
        }
    }
}
