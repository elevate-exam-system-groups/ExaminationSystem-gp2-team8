using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record GetQuestionOptionsQuery(int QuestionId)
        : IRequest<Result<List<OptionsDto>>>;

    public class GetQuestionOptionsQueryHandler
        : IRequestHandler<GetQuestionOptionsQuery, Result<List<OptionsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuestionOptionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<OptionsDto>>> Handle(
            GetQuestionOptionsQuery request,
            CancellationToken cancellationToken)
        {
            var options = await _unitOfWork.Repository<Options>()
                .GetAll(asNoTracking: true)
                .Where(o => o.QuestionId == request.QuestionId && !o.IsDeleted)
                .Select(o => new OptionsDto
                {
                    OptionId = o.Id,
                    Text = o.OptionText,
                    IsCorrect = o.IsCorrect 
                })
                .ToListAsync(cancellationToken);

            if (!options.Any())
                return Result<List<OptionsDto>>.Fail("No options found");

            return Result<List<OptionsDto>>.Success(options);
        }
    }
}