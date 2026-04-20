using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record GetQuizQuestionsQuery(int QuizId)
        : IRequest<Result<List<QuestionDto>>>;

    public class GetQuizQuestionsQueryHandler
        : IRequestHandler<GetQuizQuestionsQuery, Result<List<QuestionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuizQuestionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<QuestionDto>>> Handle(
            GetQuizQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var questions = await _unitOfWork.Repository<Question>()
                .GetAll(asNoTracking: true)
                .Where(q => q.QuizId == request.QuizId && !q.IsDeleted)
                .OrderBy(q => q.OrderIndex)
                .Select(q => new QuestionDto
                {
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText
                })
                .ToListAsync(cancellationToken);

            if (!questions.Any())
                return Result<List<QuestionDto>>.Fail("No questions found");

            return Result<List<QuestionDto>>.Success(questions);
        }
    }
}