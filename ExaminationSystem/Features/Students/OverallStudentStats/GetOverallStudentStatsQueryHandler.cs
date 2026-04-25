using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Features.Students.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Students.OverallStudentStats
{
    public class GetOverallStudentStatsQueryHandler : IRequestHandler<GetOverallStudentStatsQuery, StudentStatsDto>
    {
        private readonly IGeneralRepository<Domain.Entities.Attempts> _repository;

        public GetOverallStudentStatsQueryHandler(IGeneralRepository<Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public async Task<StudentStatsDto> Handle(GetOverallStudentStatsQuery request, CancellationToken cancellationToken)
        {
            var attempts = _repository.GetAllAsync(a => a.UserId == request.studentId);

            var countAttempts = await attempts.CountAsync();

            var avgScore = countAttempts == 0
                ? 0
                : await attempts.AverageAsync(a => a.ScorePct ?? 0);

            var passedAttempts = await attempts.CountAsync(a => a.Passed == true);
            var passRate = countAttempts == 0
                ? 0
                : (double)passedAttempts / countAttempts * 100;

            return new StudentStatsDto(countAttempts, avgScore, passRate);
        }
    }
}
