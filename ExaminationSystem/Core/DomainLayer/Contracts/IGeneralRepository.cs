using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface IGeneralRepository<TEntity>  where TEntity :class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IQueryable<TEntity>> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);

    }
}
