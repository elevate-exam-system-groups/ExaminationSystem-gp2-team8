using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ExaminationSystem.Infrastructure.Persistence.Reposteries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<string,object> _repositories;
        public UnitOfWork(StoreDbContext dbContext) 
        {
            _dbContext = dbContext;
            _repositories = new();
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var key = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(key))
                _repositories[key] = new GenericRepository<TEntity>(_dbContext);
            return (IGenericRepository<TEntity>)_repositories[key];
        }

        public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
    }
}
