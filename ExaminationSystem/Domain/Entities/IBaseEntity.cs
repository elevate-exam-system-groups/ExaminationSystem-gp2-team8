namespace ExaminationSystem.Domain.Entities
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }

    }
}
