namespace ExaminationSystem.Domain.Entities
{
    public class BaseEntity
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
