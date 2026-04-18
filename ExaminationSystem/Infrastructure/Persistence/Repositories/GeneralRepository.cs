using ExaminationSystem.BuildingBlocks.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence.Repositories
{
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class, Domain.Entities.IBaseEntity
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
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _dbcontext.Set<TEntity>().Update(entity);
        }

        public async Task<List<TEntity>> GetAllAsync() =>
            await _dbcontext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id) =>
            await _dbcontext.Set<TEntity>().FindAsync(id);

        public IQueryable<TEntity> Query() => _dbcontext.Set<TEntity>().AsQueryable();

        public async Task<int> SaveChangesAsync() =>
           await _dbcontext.SaveChangesAsync();

        public void Update(TEntity entity)=> _dbcontext.Set<TEntity>().Update(entity);
    }
}
