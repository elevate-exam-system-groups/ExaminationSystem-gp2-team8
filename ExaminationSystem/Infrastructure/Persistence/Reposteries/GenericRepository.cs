using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence.Reposteries
{
    public class GenericRepository<TEntity>(StoreDbContext _dbContext) 
        : IGenericRepository<TEntity> where TEntity : class
    {
        public async Task AddAsync(TEntity entity)
        => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
        {
            if (entity is not IBaseEntity softDeletableEntity)
            {
                throw new InvalidOperationException($"{typeof(TEntity).Name} must implement IBaseEntity to support soft delete.");
            }

            softDeletableEntity.IsDeleted = true;
            softDeletableEntity.DeletedAt = DateTime.UtcNow;
            _dbContext.Set<TEntity>().Update(entity);
        }
           
        

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking=false)
        =>  asNoTracking ? await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync() :
            await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id)
        => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
         =>  _dbContext.Set<TEntity>().Update(entity);
    }
}
