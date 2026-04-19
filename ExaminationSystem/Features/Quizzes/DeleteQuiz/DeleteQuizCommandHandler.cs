using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.DeleteQuiz
{
    public class DeleteQuizCommandHandler : IRequestHandler<DeleteQuizCommand, bool>
    {
        private readonly IGeneralRepository<Quiz> _repository;

        public DeleteQuizCommandHandler(IGeneralRepository<Quiz> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.GetByIdAsync(request.quizId);
            ValidateRequest(quiz);

            //if (quiz.Status == Domain.Enums.Status.published) return ApiResponse<bool>.FailureResponse("Can not delete a published quiz");

            _repository.Delete(quiz!);
            await _repository.SaveChangesAsync();

            return true;

        }

        private static void ValidateRequest(Quiz? quiz)
        {
            if (quiz is null)
            {
                throw new NotFoundException("Quiz Not Found");
            }
        }
    }
}
