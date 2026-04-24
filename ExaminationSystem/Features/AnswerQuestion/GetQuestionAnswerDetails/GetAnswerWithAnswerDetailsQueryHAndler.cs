using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AnswerQuestion.GetQuestionAnswerDetails
{
    public class GetAnswerWithAnswerDetailsQueryHAndler : IRequestHandler<GetAnswerWithAnswerDetailsQuery, IEnumerable<GetAnswerDetailsDTO>>
    {
        private readonly IGeneralRepository<StudentAnswer> _repository;

        public GetAnswerWithAnswerDetailsQueryHAndler(IGeneralRepository<StudentAnswer> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<GetAnswerDetailsDTO>> Handle(GetAnswerWithAnswerDetailsQuery request, CancellationToken cancellationToken)
        {
            var answers = await _repository.Query()
                 .AsNoTracking()
                 .Where(sa => sa.AttemptId == request.attemptId)
                 .ToListAsync(cancellationToken);

            //return 
            return answers.Select(a => new GetAnswerDetailsDTO
            {
                QuestionId = a.QuestionId,
                QuestionText = a.Question.QuestionText,
                SelectedAnswer = a.SelectedOption.OptionText,
                IsCorrect = a.IsCorrect
            });
          
        }
    }
}
