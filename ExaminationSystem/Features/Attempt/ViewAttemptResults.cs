using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Attempt.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempt
{
    public record ViewAttemptResults(int studentId, int id) : IRequest<ApiResponse<ViewResultDTO>>;

    public class ViewAttemptResultsHandler : IRequestHandler<ViewAttemptResults, ApiResponse<ViewResultDTO>>
    {
        IGeneralRepository<ExaminationSystem.Domain.Entities.Attempts> _repository;
        public ViewAttemptResultsHandler(IGeneralRepository<ExaminationSystem.Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public async Task<ApiResponse<ViewResultDTO>> Handle(ViewAttemptResults request, CancellationToken cancellationToken)
        {
            var exist =await _repository.GetByIdAsync(request.id);


            if (exist == null) throw new NotFoundException("this attempts is not Founded");

            var attempt = await _repository.GetAll()
                .Include(a => a.StudentAnswers)
                    .ThenInclude(s => s.SelectedOption)
                .Include(a => a.StudentAnswers)
                    .ThenInclude(s => s.Question)
                        .ThenInclude(q => q.Options)
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Questions)
                .Where(a => a.Id == request.id
                    && (a.Attempt == AttemptStatus.TimeOut || a.Attempt == AttemptStatus.Submit)
                    && a.UserId == request.studentId)
                .FirstOrDefaultAsync(cancellationToken);

            if (attempt is null) throw new ForbiddenException(" results not available until submitted ");


            var Status = attempt.Quiz.PassScore <= attempt.score ? "Passed" : "Failed";

            var questionsCount=attempt.Quiz.Questions.Count;

            var correctCount=attempt.StudentAnswers.Count(a=>a.SelectedOption.IsCorrect==true);

          


            var attemptDto = new ViewResultDTO()
            {
                score = attempt.score,
                status = Status,
                TotalQuestions = questionsCount,
                CorrectCount = correctCount,
                // i wanna get list of questions for this attempt
                Questions = attempt.StudentAnswers.Select(s =>  new QuestionAttemptDTO() {

                    QuestionId = s.QuestionId,
                    studentAnswer = s.SelectedOption.OptionText,
                    CorrectAnswer = s.Question.Options.FirstOrDefault(o => o.IsCorrect == true)?.OptionText ?? "",
                    IsCorrect = s.SelectedOption.IsCorrect

                }).ToList()

            };

            return  ApiResponse<ViewResultDTO>.SuccessResponse(attemptDto);
        }
    }


}
