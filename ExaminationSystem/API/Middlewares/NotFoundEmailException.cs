namespace ExaminationSystem.API.Middlewares
{
    public class NotFoundEmailException : NotFoundException
    {
        public NotFoundEmailException(string Email) : base($"No account found with this : {Email}")
        {
        }
    }
}
