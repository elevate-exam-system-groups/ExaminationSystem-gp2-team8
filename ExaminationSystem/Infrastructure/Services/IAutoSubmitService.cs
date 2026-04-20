using ExaminationSystem.Domain.Entities;

namespace ExaminationSystem.Infrastructure.Services
{
    public interface IAutoSubmitService
    {
        /// <summary>
        /// Scores the attempt from whatever answers are on record,
        /// sets status = TimedOut, and persists. Called when the server
        /// detects the deadline has passed on any attempt-related request.
        /// </summary>
        Task AutoSubmitAsync(Attempts attempt, CancellationToken cancellationToken = default);
    }
}
