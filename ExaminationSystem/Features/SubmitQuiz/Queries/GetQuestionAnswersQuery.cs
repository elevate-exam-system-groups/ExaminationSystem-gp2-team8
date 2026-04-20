using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record GetQuestionAnswersQuery (int QuestionId) : IRequest<Result<AnswerDto>>;

    public class GetQuestionAnswersQueryHandler : IRequestHandler<GetQuestionAnswersQuery, Result<AnswerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuestionAnswersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<AnswerDto>> Handle(
            GetQuestionAnswersQuery request,
            CancellationToken cancellationToken)
        {
        
            var question = await _unitOfWork.Repository<Question>()
                .GetAll()
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == request.QuestionId && !q.IsDeleted, cancellationToken);

         
            if (question == null)
                return Result<AnswerDto>.Fail("Question not found");

          
            var correctOption = question.Options
                .FirstOrDefault(o => o.IsCorrect && !o.IsDeleted);

           
            if (correctOption == null)
                return Result<AnswerDto>.Fail("Correct answer not found");

       
            var result = new AnswerDto
            {
                QuestionId = question.Id,
                CorrectOptionId = correctOption.Id
            };

          
            return Result<AnswerDto>.Success(result);
        

    }
}
}
