using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Commands
{
    public record SaveStudentAnswersCommand(
           int UserId,
           int AttemptId,
           ICollection<QuizAnswerDto> Answers
       ) : IRequest<Result<SaveStudentAnswersResponseDto>>;

    public class SaveStudentAnswersCommandHandler
        : IRequestHandler<SaveStudentAnswersCommand, Result<SaveStudentAnswersResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveStudentAnswersCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SaveStudentAnswersResponseDto>> Handle(
            SaveStudentAnswersCommand request,
            CancellationToken cancellationToken)
        {
            
            var studentAnswers = request.Answers.Select(a => new StudentAnswer
            {
                UserId = request.UserId,
                AttemptId = request.AttemptId,
                QuestionId = a.QuestionId,
                SelectedOptionId = a.SelectedOptionId
            });

           
            foreach (var answer in studentAnswers)
            {
                await _unitOfWork.Repository<StudentAnswer>().AddAsync(answer);
            }

            
            await _unitOfWork.SaveChangesAsync();

           
            var response = new SaveStudentAnswersResponseDto
            {
                AttemptId = request.AttemptId,
                SavedAnswersCount = request.Answers.Count
            };

            return Result<SaveStudentAnswersResponseDto>.Success(response);
        }
    }
}
