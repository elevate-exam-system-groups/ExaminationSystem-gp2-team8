using System.Linq.Expressions;

namespace ExaminationSystem.BuildingBlocks.Interfaces
{
    public interface IGeneralRepository<TEntity> where TEntity : class, Domain.Entities.IBaseEntity
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        public IQueryable<TEntity> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> Query();
        Task<int> SaveChangesAsync();
    }
}
