using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.DeleteDiploma
{
    public class DeleteDiplomaCommandHandler : IRequestHandler<DeleteDiplomaCommand, ApiResponse<bool>>
    {
        private readonly ExamAppDbContext _dbContext;

        public DeleteDiplomaCommandHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<bool>> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _dbContext.Diplomas
                .Include(d => d.Enrollments)
                .FirstOrDefaultAsync(d => d.Id == request.id, cancellationToken);

            if (diploma == null) throw new NotFoundException("Diploma Not Found");

            if (diploma.Enrollments.Any())
            {
                return ApiResponse<bool>.FailureResponse("Cannot delete diploma with active enrollments", "409");
            }

            diploma.IsDeleted = true;
            diploma.DeletedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
