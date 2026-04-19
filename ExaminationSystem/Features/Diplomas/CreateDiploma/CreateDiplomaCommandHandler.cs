using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Diplomas.DTOS;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.CreateDiploma
{
    public class CreateDiplomaCommandHandler : IRequestHandler<CreateDiplomaCommand, CreateDiplomaDto>
    {
        private readonly IGeneralRepository<Diploma> _repository;

        public CreateDiplomaCommandHandler(IGeneralRepository<Diploma> repository)
        {
            _repository = repository;
        }
        public async Task<CreateDiplomaDto> Handle(CreateDiplomaCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var diploma = new Diploma
            {
                Title = request.Title.Trim(),
                Description = request.Descreption?.Trim(),
                status = Domain.Enums.Status.Draft,
                CreatedAt = DateTime.UtcNow,
            };

            await _repository.AddAsync(diploma);
            await _repository.SaveChangesAsync();

            return new CreateDiplomaDto
            {
                Title = diploma.Title,
                Description = diploma.Description,
            };
        }

        private static void ValidateRequest(CreateDiplomaCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ValidationException("Diploma title is required");
            }
        }
    }
}
