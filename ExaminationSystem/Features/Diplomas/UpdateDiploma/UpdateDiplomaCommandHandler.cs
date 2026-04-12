using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.UpdateDiploma
{
    public class UpdateDiplomaCommandHandler : IRequestHandler<UpdateDiplomaCommand, ApiResponse<UpdateDiplomaDto>>
    {
        private readonly ExamAppDbContext _dbContext;

        public UpdateDiplomaCommandHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<UpdateDiplomaDto>> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _dbContext.Diplomas.FindAsync(request.id);
            if (diploma == null) return ApiResponse<UpdateDiplomaDto>.FailureResponse("Diploma Not Found", "404");

            diploma.Title = request.title;
            diploma.Description = request.description;

            _dbContext.Update(diploma);
            await _dbContext.SaveChangesAsync();

            return ApiResponse<UpdateDiplomaDto>.SuccessResponse(new UpdateDiplomaDto() {Id = diploma.Id, Title = diploma.Title, Description = diploma.Description});
        }
    }
}
