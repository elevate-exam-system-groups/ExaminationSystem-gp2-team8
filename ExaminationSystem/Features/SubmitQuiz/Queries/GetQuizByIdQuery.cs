using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record GetQuizByIdQuery(int QuizId) 
        : IRequest<Result<QuizDto>>;

    public class GetQuizByIdQueryHandler: IRequestHandler<GetQuizByIdQuery, Result<QuizDto>>

    {
        private readonly IUnitOfWork _unitOfWork;
        public GetQuizByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<QuizDto>> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _unitOfWork.Repository<Quiz>().GetByIdAsync(request.QuizId);   

            if (quiz == null)
                return Result<QuizDto>.Fail("Quiz not found");
            return Result<QuizDto>.Success(new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                DurationMinutes = quiz.DurationMinutes,
                PassScore = quiz.PassScore,
                MaxAttempts = quiz.MaxAttempts,
                Status = quiz.Status,
                Instructions = quiz.Instructions
            }); 

        }
    }

}
