using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExaminationSystem.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity>(ExamAppDbContext _dbContext) 
        : IGenericRepository<TEntity> where TEntity : class
    {
        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
         => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(predicate);
        }

        public IQueryable<TEntity> GetAll(bool asNoTracking=false)
        {
               var query = _dbContext.Set<TEntity>().AsQueryable();
            if (asNoTracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
         =>  _dbContext.Set<TEntity>().Update(entity);
    }
}
