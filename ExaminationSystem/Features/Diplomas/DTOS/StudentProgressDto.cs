namespace ExaminationSystem.Features.Diplomas.DTOS
{
    public class StudentProgressDto
    {
        public int CompletedQuizzes { get; set; }   // عدد الكويزات اللي عدى فيها
        public int TotalAttempts { get; set; }       // عدد المحاولات الكلية
        public double AverageScore { get; set; }     // متوسط الدرجات
    }
}
