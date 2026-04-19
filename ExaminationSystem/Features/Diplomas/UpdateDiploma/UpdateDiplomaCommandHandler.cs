using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.UpdateDiploma
{
    public class UpdateDiplomaCommandHandler : IRequestHandler<UpdateDiplomaCommand, UpdateDiplomaDto>
    {
        private readonly IGeneralRepository<Diploma> _repository;

        public UpdateDiplomaCommandHandler(IGeneralRepository<Diploma> repository)
        {
            _repository = repository;
        }
        public async Task<UpdateDiplomaDto> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var diploma = await _repository.GetByIdAsync(request.id)
                ?? throw new NotFoundException("Diploma Not Found");

            diploma.Title = request.title.Trim();
            diploma.Description = request.description?.Trim();

            _repository.Update(diploma);
            await _repository.SaveChangesAsync();

            return MapToDto(diploma);
        }

        private static void ValidateRequest(UpdateDiplomaCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.title))
            {
                throw new ValidationException("Diploma title is required");
            }
        }

        private static UpdateDiplomaDto MapToDto(Diploma diploma)
        {
            return new UpdateDiplomaDto
            {
                Id = diploma.Id,
                Title = diploma.Title,
                Description = diploma.Description
            };
        }
    }
}
