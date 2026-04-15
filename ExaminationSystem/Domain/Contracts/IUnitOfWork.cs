namespace ExaminationSystem.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}
