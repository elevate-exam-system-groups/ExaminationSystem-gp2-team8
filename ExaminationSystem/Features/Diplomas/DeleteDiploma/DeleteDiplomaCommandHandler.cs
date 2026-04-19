using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.DeleteDiploma
{
    public class DeleteDiplomaCommandHandler : IRequestHandler<DeleteDiplomaCommand, bool>
    {
        private readonly IGeneralRepository<Domain.Entities.Diploma> _repository;

        public DeleteDiplomaCommandHandler(IGeneralRepository<Domain.Entities.Diploma> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var diploma = await _repository.GetByIdAsync(request.id)
                ?? throw new NotFoundException("Diploma Not Found");

            _repository.Delete(diploma);
            await _repository.SaveChangesAsync();

            return true;
        }

        private async Task ValidateRequestAsync(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diplomaState = await _repository.Query()
                .AsNoTracking()
                .Where(d => d.Id == request.id)
                .Select(d => new DiplomaDeleteState(d.Id, d.Enrollments.Any()))
                .FirstOrDefaultAsync(cancellationToken);

            if (diplomaState is null)
            {
                throw new NotFoundException("Diploma Not Found");
            }

            if (diplomaState.HasEnrollments)
            {
                throw new ConflictException("Cannot delete diploma with active enrollments");
            }
        }

        private sealed record DiplomaDeleteState(int Id, bool HasEnrollments);
    }
}
