using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.UpdateQuiz
{
    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, CreateQuizResponse>
    {
        private readonly IGeneralRepository<Quiz> _quizRepository;
        private readonly IGeneralRepository<Diploma> _diplomaRepository;

        public UpdateQuizCommandHandler(
            IGeneralRepository<Quiz> quizRepository,
            IGeneralRepository<Diploma> diplomaRepository)
        {
            _quizRepository = quizRepository;
            _diplomaRepository = diplomaRepository;
        }
        public async Task<CreateQuizResponse> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var quiz = await _quizRepository.Query()
                .FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken)
                ?? throw new NotFoundException("Quiz Not Found");

            quiz.Title = request.dto.Title.Trim();
            quiz.DiplomaId = request.dto.DiplomaId;
            quiz.DurationMinutes = request.dto.DurationMinutes;
            quiz.Instructions = request.dto.Instructions?.Trim();
            quiz.MaxAttempts = request.dto.MaxAttempts;
            quiz.PassScore = request.dto.PassScore;

            _quizRepository.Update(quiz);
            await _quizRepository.SaveChangesAsync();

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

        private async Task ValidateRequestAsync(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            ValidateQuiz(request.dto);

            var quizExists = await _quizRepository.Query()
                .AsNoTracking()
                .AnyAsync(q => q.Id == request.QuizId, cancellationToken);

            if (!quizExists)
            {
                throw new NotFoundException("Quiz Not Found");
            }

            var diplomaExists = await _diplomaRepository.Query()
                .AsNoTracking()
                .AnyAsync(d => d.Id == request.dto.DiplomaId, cancellationToken);

            if (!diplomaExists)
            {
                throw new NotFoundException("Diploma Not Found");
            }
        }

        private static void ValidateQuiz(UpdateQuizDto dto)
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
    }
}
