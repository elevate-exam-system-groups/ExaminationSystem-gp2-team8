namespace ExaminationSystem.Domain.Entities
{
    public class Student : User
    {
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Attempts> Attempts { get; set; } = new List<Attempts>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
