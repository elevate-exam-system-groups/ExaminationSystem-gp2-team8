namespace ExaminationSystem.BuildingBlocks.Interfaces
{
    public interface IGeneralRepository<TEntity> where TEntity : class, Domain.Entities.IBaseEntity
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        public IQueryable<TEntity> Query();
        Task<int> SaveChangesAsync();
    }
}
