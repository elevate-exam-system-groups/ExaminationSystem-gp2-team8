namespace ExaminationSystem.Domain.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int DiplomaId { get; set; }
        public Diploma Diploma { get; set; }=null!;
        public DateTime EnrollmentDate { get; set; }
    }
}
