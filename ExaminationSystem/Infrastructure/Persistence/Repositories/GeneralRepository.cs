using ExaminationSystem.BuildingBlocks.Interfaces;

namespace ExaminationSystem.Infrastructure.Persistence.Repositories
{
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class
    {
        private readonly ExamAppDbContext _dbcontext;

        public GeneralRepository(ExamAppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Add(TEntity entity)=> _dbcontext.Set<TEntity>().Add(entity);


        public void Delete(TEntity entity) => _dbcontext.Set<TEntity>().Remove(entity);

        public IQueryable<TEntity> GetAll() => _dbcontext.Set<TEntity>();

        public async Task<TEntity?> GetByIdAsync(int id)=> await _dbcontext.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)=> _dbcontext.Set<TEntity>().Update(entity);
        
    }
}
