using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.StartQuiz.Queries;
using ExaminationSystem.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StartQuiz.Commands
{
    public record StartQuizOrchestrator(int quizId, int userId) : IRequest<Result<QuizAttemptDto>>;

    public class StartQuizOrchestratorHandler: IRequestHandler<StartQuizOrchestrator, Result<QuizAttemptDto>>
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly ExamAppDbContext _dbContext;

        public StartQuizOrchestratorHandler( IMediator mediator,UserManager<User> userManager,ExamAppDbContext dbContext)
        {
            _mediator = mediator;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<Result<QuizAttemptDto>> Handle(StartQuizOrchestrator request,CancellationToken cancellationToken){
            // 1. check quiz availability 
            var isAvailable = await _mediator.Send(new IsQuizAvailableQuery(request.quizId) , cancellationToken);

            if (!isAvailable)
                return Result<QuizAttemptDto>.Fail("Quiz not available");

            // 2. get quiz details
            var quiz = await _dbContext.Quizzes.FirstOrDefaultAsync(q => q.Id == request.quizId, cancellationToken);

            if (quiz == null)
                return Result<QuizAttemptDto>.Fail("Quiz not found");

            // 3. get user
            var user = await _userManager.FindByIdAsync(request.userId.ToString());

            if (user == null || user.Status != UserStatus.Active)
                return Result<QuizAttemptDto>.Fail("User not found or inactive");

            // 4. max attempts
            if (quiz.MaxAttempts.HasValue)
            {
                var userAttemptsCount = await _dbContext.Attempts
                    .CountAsync(a =>a.UserId == request.userId && a.QuizId == request.quizId, cancellationToken);

                if (userAttemptsCount >= quiz.MaxAttempts.Value)
                    return Result<QuizAttemptDto>.Fail("Max attempts reached");
            }

            // 5. active attempt
            var hasActiveAttempt = await _dbContext.Attempts.AnyAsync(a =>
                    a.UserId == request.userId &&
                    a.QuizId == request.quizId &&
                    a.Attempt == AttemptStatus.InProgress, cancellationToken);

            if (hasActiveAttempt)
                return Result<QuizAttemptDto>.Fail("You already have an active attempt");

            // 6. create attempt
            var attempt = new Attempts
            {
                QuizId = quiz.Id,
                UserId = user.Id,
                StartTime = DateTime.UtcNow,
                Deadline = DateTime.UtcNow.AddMinutes(quiz.DurationMinutes),
                Attempt = AttemptStatus.InProgress
            };

            await _dbContext.Attempts.AddAsync(attempt, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // 7. dto
            var dto = new QuizAttemptDto
            {
                AttemptId = attempt.Id,
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,
                DurationMinutes = quiz.DurationMinutes,
                Instructions = quiz.Instructions,
                StartTime = attempt.StartTime,
                Deadline = attempt.Deadline,
                Status = attempt.Attempt.ToString()
            };

            return Result<QuizAttemptDto>.Success(dto, "Quiz started successfully");
        }
    }
    }

