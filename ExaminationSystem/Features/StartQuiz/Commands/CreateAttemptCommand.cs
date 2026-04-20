using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.StartQuiz.DTOS;
using MediatR;

public record CreateAttemptCommand(int UserId, int QuizId, int DurationMinutes)
    : IRequest<Result<AttemptDto>>;

public class CreateAttemptCommandHandler
    : IRequestHandler<CreateAttemptCommand, Result<AttemptDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAttemptCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AttemptDto>> Handle(
        CreateAttemptCommand request,
        CancellationToken cancellationToken)
    {
        var attempt = new Attempts
        {
            UserId = request.UserId,
            QuizId = request.QuizId,
            StartTime = DateTime.UtcNow,
            Deadline = DateTime.UtcNow.AddMinutes(request.DurationMinutes),
            Attempt = AttemptStatus.InProgress
        };

        await _unitOfWork.Repository<Attempts>().AddAsync(attempt);
        await _unitOfWork.SaveChangesAsync();

        return Result<AttemptDto>.Success(new AttemptDto
        {
            AttemptId = attempt.Id,
            StartTime = attempt.StartTime,
            Deadline = attempt.Deadline,
            Status = attempt.Attempt.ToString()
        });
    }
}

