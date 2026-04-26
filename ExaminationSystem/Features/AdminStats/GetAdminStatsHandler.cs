using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.AdminStats.DTOs;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
namespace ExaminationSystem.Features.AdminStats
{
    public class GetAdminStatsHandler : IRequestHandler<GetAdminStatsQuery, Result<AdminStatsDto>>
    {
        private readonly IGeneralRepository<User> _userrepository;
        private readonly IGeneralRepository<Diploma> _diplomarepository;
        private readonly IGeneralRepository<Quiz> _quizrepository;
        private readonly IGeneralRepository<ExaminationSystem.Domain.Entities.Attempts> _attemptsrepository;
        //private readonly ExamAppDbContext _db;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GetAdminStatsHandler> _logger;

        private const string CacheKey = "admin:stats";
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

        public GetAdminStatsHandler(
            IGeneralRepository<User> userrepository,
            IGeneralRepository<ExaminationSystem.Domain.Entities.Attempts> attemptsrepository,
            IGeneralRepository<Diploma> diplomarepository,
            IGeneralRepository<Quiz> quizrepository,
            IMemoryCache cache,
            ILogger<GetAdminStatsHandler> logger)
        {
            _userrepository = userrepository;
            _attemptsrepository = attemptsrepository;
            _diplomarepository = diplomarepository;
            _quizrepository = quizrepository;
            _cache = cache;
            _logger = logger;
           
        }
        public async Task<Result<AdminStatsDto>> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
        {
            // Try cache first
            if (_cache.TryGetValue(CacheKey, out AdminStatsDto? cached) && cached is not null)
            {
                _logger.LogDebug("Admin stats served from cache.");
                return Result<AdminStatsDto>.Success(cached);
            }
            // All aggregations run as DB-side queries (no in-memory counting)

            // Total non-deleted users across all roles
            var totalUsers =  await _userrepository.Query().CountAsync(cancellationToken);

            // Active users today — distinct users with a successful login since midnight UTC
            var todayUtc = DateTime.UtcNow.Date; // 2026-04-25 00:00:00 UTC
            var activeUsersToday = await _attemptsrepository.Query()
                .Where(l => l.CreatedAt >= todayUtc)
                .Select(l => l.UserId)
                .Distinct()
                .CountAsync(cancellationToken);

            // Total diplomas (global query filter already excludes soft-deleted)
            var totalDiplomas = await _diplomarepository.Query().CountAsync(cancellationToken);

            // Total quizzes (global query filter already excludes soft-deleted)
            var totalQuizzes = await _quizrepository.Query().CountAsync(cancellationToken);

            // Total attempts across all students and all statuses
            var totalAttempts = await _attemptsrepository.Query().CountAsync(cancellationToken);

            // Pass rate — only from completed attempts (Submitted or TimedOut)
            // Uses DB aggregation: two counts in one round trip
            var completedStats = await _attemptsrepository.Query()
                .Where(a => a.Attempt == AttemptStatus.Submit|| a.Attempt == AttemptStatus.TimeOut)
                .GroupBy(_ => 1)  // collapse to one row
                .Select(g => new
                {
                    Total = g.Count(),
                    Passed = g.Count(a => a.Passed == true),
                })
                .FirstOrDefaultAsync(cancellationToken);

            var avgPassRate = completedStats is { Total: > 0 } ?
                Math.Round((double)completedStats.Passed / completedStats.Total * 100, 2): 0.0;

            
            var stats = new AdminStatsDto
            {
                TotalUsers = totalUsers,
                ActiveUsersToday = activeUsersToday,
                TotalDiplomas = totalDiplomas,
                TotalQuizzes = totalQuizzes,
                TotalAttempts = totalAttempts,
                AvgPassRate = avgPassRate,
                GeneratedAt = DateTime.UtcNow,
            };

            if (stats is null)
                return Result<AdminStatsDto>.Failure("No data found.", 404);
            // Store in cache 
            _cache.Set(CacheKey, stats, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheTtl,
                // Low priority — evict first under memory pressure
                Priority = CacheItemPriority.Low,
            });

            _logger.LogInformation("Admin stats computed and cached. Users={U}, Active={A}, PassRate={P}%",totalUsers, activeUsersToday, avgPassRate);

            return Result<AdminStatsDto>.Success(stats);
        }
    }
}
