using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.CreateDiploma
{
    public class CreateDiplomaCommandHandler : IRequestHandler<CreateDiplomaCommand, ApiResponse<CreateDiplomaDto>>
    {
        private readonly ExamAppDbContext _dbContext;

        public CreateDiplomaCommandHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<CreateDiplomaDto>> Handle(CreateDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = new Diploma
            {
                Title = request.Title,
                Description = request.Descreption,
                status = Domain.Enums.Status.Draft,
                CreatedAt = DateTime.UtcNow,
            };

            _dbContext.Diplomas.Add(diploma);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0 )
                return ApiResponse<CreateDiplomaDto>.SuccessResponse(new CreateDiplomaDto
                {
                    Title = request.Title,
                    Description = request.Descreption,
                });

            return ApiResponse<CreateDiplomaDto>.FailureResponse("Error: Can't Create Diploma");
        }
    }
}
