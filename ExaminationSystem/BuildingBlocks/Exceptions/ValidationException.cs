namespace ExaminationSystem.BuildingBlocks.Exceptions
{
    public class ValidationException(string message, List<string>? errors = null)
        : Exception(message)
    {
        public List<string> Errors { get; } = errors ?? [];
    }
}