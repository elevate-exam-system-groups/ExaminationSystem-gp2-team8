using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.AnswerQuestion.DTOs;
using MediatR;

namespace ExaminationSystem.Features.AnswerQuestion
{
    public record SubmitAnswerCommand(int AttemptId,
        int CurrentUserId,
        SubmitAnswerRequestDto Dto) : IRequest<ApiResponse<SubmitAnswerResponseDto>>;

}
