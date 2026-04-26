using System.Linq.Expressions;

namespace ExaminationSystem.Domain.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool asNoTracking=false);
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity);
        void  Delete(TEntity entity);
    }
} 
