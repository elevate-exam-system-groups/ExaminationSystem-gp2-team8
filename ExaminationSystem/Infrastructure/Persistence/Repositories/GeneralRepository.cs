using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence.Repositories
{
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class
    {
        private readonly ExamAppDbContext _dbcontext;

        public GeneralRepository(ExamAppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddAsync(TEntity entity) =>
            await _dbcontext.Set<TEntity>().AddAsync(entity);


        public void Delete(TEntity entity)
        {
            if (entity is not IBaseEntity softDeletableEntity)
            {
                throw new InvalidOperationException($"{typeof(TEntity).Name} must implement IBaseEntity to support soft delete.");
            }

            softDeletableEntity.IsDeleted = true;
            softDeletableEntity.DeletedAt = DateTime.UtcNow;
            _dbcontext.Set<TEntity>().Update(entity);
        }

        public async Task<List<TEntity>> GetAllAsync() =>
            await _dbcontext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id) =>
            await _dbcontext.Set<TEntity>()
                .FirstOrDefaultAsync(entity => EF.Property<int>(entity, nameof(IBaseEntity.Id)) == id);

        public IQueryable<TEntity> Query() => _dbcontext.Set<TEntity>().AsQueryable();

        public async Task<int> SaveChangesAsync() =>
            await _dbcontext.SaveChangesAsync();

        public void Update(TEntity entity)=> _dbcontext.Set<TEntity>().Update(entity);
    }
}
